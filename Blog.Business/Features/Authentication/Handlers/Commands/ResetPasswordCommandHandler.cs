using Blog.Business.Constants;
using Blog.Business.Features.Authentication.Commands;
using Blog.Business.Features.Authentication.ValidationRules;
using Blog.Core.Aspects.Autofac.Logger;
using Blog.Core.Aspects.Autofac.Validation;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Blog.Core.Utilities.Results;
using Blog.Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Business.Features.Authentication.Handlers.Commands
{
	/// <summary>
	///     Reset password with the token of reset password created by forgot password
	/// </summary>
	public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, IResult>
	{
		private readonly UserManager<User> _userManager;

		public ResetPasswordCommandHandler(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		/// <summary>
		///     Reset password
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[ValidationAspect(typeof(ResetPasswordValidator))]
		[LogAspect(typeof(FileLogger))]
		public async Task<IResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByNameAsync(request.Username);
			if (user is null) return new ErrorResult(Messages.UserNotFound);
			request.ResetPasswordToken =
				Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.ResetPasswordToken));
			var result = await _userManager.ResetPasswordAsync(user, request.ResetPasswordToken, request.Password);
			return result.Succeeded
				? new SuccessResult(Messages.PasswordHasBeenResetSuccessfully)
				: new ErrorResult(Messages.PasswordResetFailed);
		}
	}
}