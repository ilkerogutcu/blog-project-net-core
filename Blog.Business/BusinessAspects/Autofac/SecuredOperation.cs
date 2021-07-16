﻿using Blog.Business.Constants;
using Blog.Core.Utilities.Interceptors;
using Blog.Core.Utilities.IoC;
using Blog.Entities.Enums;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Blog.Business.BusinessAspects.Autofac
{
	/// <summary>
	/// This Aspect control the user's roles in HttpContext by inject the IHttpContextAccessor.
	/// It is checked by writing as [SecuredOperation] on the handler.
	/// If a valid authorization cannot be found in aspect, it throws an exception.
	/// </summary>
	public class SecuredOperation : MethodInterception
	{
		private readonly Roles[] _roles;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public SecuredOperation(params Roles[] roles)
		{
			_roles = roles;
			_httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
		}

		protected override void OnBefore(IInvocation invocation)
		{
			if (_roles.Any(role => _roles.Any(x => _httpContextAccessor.HttpContext.User.IsInRole(role.ToString()))))
			{
				return;
			}

			throw new Exception(Messages.AuthorizationsDenied);
		}
	}
}