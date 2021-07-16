﻿using Blog.Core.Utilities.Results;
using MediatR;

namespace Blog.Business.Features.Authentication.Commands
{
	/// <summary>
	///     Forgot password
	/// </summary>
	public class ForgotPasswordCommand : IRequest<IResult>
	{
		/// <summary>
		///     Username
		/// </summary>
		public string Username { get; set; }
	}
}