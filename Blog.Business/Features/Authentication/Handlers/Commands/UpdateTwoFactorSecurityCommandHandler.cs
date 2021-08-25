using System;
using System.Security.Claims;
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
using Blog.Core.Aspects.Autofac.Exception;
using Microsoft.AspNetCore.Http;

namespace Blog.Business.Features.Authentication.Handlers.Commands
{
	/// <summary>
	/// Update two factor security
	/// </summary>
	public class UpdateTwoFactorSecurityCommandHandler : IRequestHandler<UpdateTwoFactorSecurityCommand, IResult>
	{
		private readonly UserManager<User> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UpdateTwoFactorSecurityCommandHandler(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
		{
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
		}

		[LogAspect(typeof(FileLogger))]
		[ExceptionLogAspect(typeof(FileLogger))]
		public async Task<IResult> Handle(UpdateTwoFactorSecurityCommand request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByIdAsync(request.UserId);
			if (user is null)
			{
				return new ErrorResult(Messages.UserNotFound);
			}
			var currentUser = await _userManager.FindByEmailAsync(_httpContextAccessor?.HttpContext.User
				.FindFirst(ClaimTypes.Email)?.Value);
			user.TwoFactorEnabled = request.IsEnable;
			user.LastModifiedDate = DateTime.Now;
			user.LastModifiedBy = currentUser.UserName;
			var result = await _userManager.UpdateAsync(user);
			return result.Succeeded
				? new SuccessResult(Messages.UpdatedUserSuccessfully)
				: new ErrorResult(Messages.FailedToUpdateUser);
		}
	}
}