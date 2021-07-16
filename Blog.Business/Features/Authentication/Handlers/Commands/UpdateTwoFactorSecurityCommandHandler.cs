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
	public class UpdateTwoFactorSecurityCommandHandler : IRequestHandler<UpdateTwoFactorSecurityCommand, IResult>
	{
		private readonly UserManager<User> _userManager;

		public UpdateTwoFactorSecurityCommandHandler(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		[LogAspect(typeof(FileLogger))]
		public async Task<IResult> Handle(UpdateTwoFactorSecurityCommand request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByIdAsync(request.userId);
			if (user is null)
			{
				return new ErrorResult(Messages.UserNotFound);
			}
			user.TwoFactorEnabled = request.IsEnable;
			var result = await _userManager.UpdateAsync(user);
			return result.Succeeded
				? new SuccessResult(Messages.UpdatedUserSuccessfully)
				: new ErrorResult(Messages.FailedToUpdateUser);
		}
	}
}