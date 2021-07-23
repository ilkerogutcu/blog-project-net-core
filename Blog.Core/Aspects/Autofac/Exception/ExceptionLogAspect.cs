using System;
using System.Linq;
using Blog.Core.CrossCuttingConcerns.Logging;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog;
using Blog.Core.Utilities.Interceptors;
using Blog.Core.Utilities.IoC;
using Blog.Core.Utilities.Messages;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Blog.Core.Aspects.Autofac.Exception
{
	/// <summary>
	/// ExceptionLogAspect
	/// </summary>
    public class ExceptionLogAspect : MethodInterception
	{
		private readonly LoggerServiceBase _loggerServiceBase;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public ExceptionLogAspect(Type loggerService)
		{
			if (loggerService.BaseType != typeof(LoggerServiceBase))
			{
				throw new ArgumentException(AspectMessages.WrongLoggerType);
			}

			_loggerServiceBase = (LoggerServiceBase)Activator.CreateInstance(loggerService);
			_httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();

		}

		protected override void OnException(IInvocation invocation, System.Exception e)
		{
			var logDetailWithException = GetLogDetail(invocation);
			logDetailWithException.ExceptionMessage = e is AggregateException exception ? string.Join(Environment.NewLine, exception.InnerExceptions.Select(x => x.Message)) : e.Message;
			_loggerServiceBase.Error(JsonConvert.SerializeObject(logDetailWithException));
		}

		private LogDetailWithException GetLogDetail(IInvocation invocation)
		{
			var logParameters = invocation.Arguments.Select((t, i) => new LogParameter { Name = invocation.GetConcreteMethod().GetParameters()[i].Name ?? string.Empty, Value = t, Type = t.GetType().Name }).ToList();
			var logDetailWithException = new LogDetailWithException
			{
				MethodName = invocation.Method.Name,
				Parameters = logParameters,
				User = (_httpContextAccessor.HttpContext == null || _httpContextAccessor.HttpContext.User.Identity?.Name == null) ? "?" : _httpContextAccessor.HttpContext.User.Identity.Name
			};
			return logDetailWithException;
		}
	}
}