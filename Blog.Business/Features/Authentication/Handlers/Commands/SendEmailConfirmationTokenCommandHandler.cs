using Blog.Business.Abstract;
using Blog.Business.Constants;
using Blog.Business.Features.Authentication.Commands;
using Blog.Core.Aspects.Autofac.Logger;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Blog.Core.Utilities.Results;
using Blog.Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Business.Features.Authentication.Handlers.Commands
{
	public class SendEmailConfirmationTokenCommandHandler : IRequestHandler<SendEmailConfirmationTokenCommand, IResult>
	{
		private readonly UserManager<User> _userManager;
		private readonly IAuthenticationMailService _authenticationMailService;

		public SendEmailConfirmationTokenCommandHandler(UserManager<User> userManager, IAuthenticationMailService authenticationMailService)
		{
			_userManager = userManager;
			_authenticationMailService = authenticationMailService;
		}

		[LogAspect(typeof(FileLogger))]
		public async Task<IResult> Handle(SendEmailConfirmationTokenCommand request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByNameAsync(request.Username);
			if (user is null)
			{
				return new ErrorResult(Messages.UserNotFound);
			}

			var verificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			await _authenticationMailService.SendVerificationEmail(user, verificationToken);
			return new SuccessResult(Messages.SentConfirmationEmailSuccessfully);
		}
	}
}