using Blog.Business.Constants;
using Blog.Business.Features.Authentication.Commands;
using Blog.Core.Aspects.Autofac.Logger;
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
	///     Confirm email
	/// </summary>
	public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, IResult>
	{
		private readonly UserManager<User> _userManager;

		public ConfirmEmailCommandHandler(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		/// <summary>
		///     Confirm email
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[LogAspect(typeof(FileLogger))]
		public async Task<IResult> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByIdAsync(request.UserId);
			if (user is null)
			{
				return new ErrorResult(Messages.UserNotFound);
			}

			request.VerificationToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.VerificationToken));
			var result = await _userManager.ConfirmEmailAsync(user, request.VerificationToken);
			return result.Succeeded
				? new SuccessResult(Messages.EmailSuccessfullyConfirmed)
				: new ErrorResult(Messages.ErrorVerifyingMail);
		}
	}
}