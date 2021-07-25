using Blog.Business.Abstract;
using Blog.Business.Constants;
using Blog.Business.Features.Authentication.Commands;
using Blog.Core.Aspects.Autofac.Logger;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Blog.Core.Utilities.Results;
using Blog.Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Aspects.Autofac.Exception;

namespace Blog.Business.Features.Authentication.Handlers.Commands
{
	/// <summary>
	///     Send forgot password email
	/// </summary>
	public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, IResult>
	{
		private readonly IAuthenticationMailService _authenticationMailService;
		private readonly UserManager<User> _userManager;

		public ForgotPasswordCommandHandler(UserManager<User> userManager, IAuthenticationMailService authenticationMailService)
		{
			_userManager = userManager;
			_authenticationMailService = authenticationMailService;
		}

		[LogAspect(typeof(FileLogger))]
		[ExceptionLogAspect(typeof(FileLogger))]
		public async Task<IResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
		{
			try
			{
				var user = await _userManager.FindByNameAsync(request.Username);
				if (user is null)
				{
					return new ErrorResult(Messages.UserNotFound);
				}

				var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
				await _authenticationMailService.SendForgotPasswordEmail(user, resetToken);
				return new SuccessResult(Messages.SentForgotPasswordEmailSuccessfully);
			}
			catch (Exception e)
			{
				return new ErrorResult(Messages.ForgotPasswordFailed);
			}
		}
	}
}