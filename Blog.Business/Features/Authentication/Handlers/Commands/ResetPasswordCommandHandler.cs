using System;
using System.Security.Claims;
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
using Blog.Core.Aspects.Autofac.Exception;
using Microsoft.AspNetCore.Http;

namespace Blog.Business.Features.Authentication.Handlers.Commands
{
    /// <summary>
    ///     Reset password with the token of reset password created by forgot password
    /// </summary>
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, IResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResetPasswordCommandHandler(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        ///     Reset password
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ValidationAspect(typeof(ResetPasswordValidator))]
        [LogAspect(typeof(FileLogger))]
        [ExceptionLogAspect(typeof(FileLogger))]
        public async Task<IResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            var currentUser = await _userManager.FindByEmailAsync(_httpContextAccessor?.HttpContext.User
                .FindFirst(ClaimTypes.Email)?.Value);
            if (user is null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }

            request.ResetPasswordToken =
                Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.ResetPasswordToken));
            var result = await _userManager.ResetPasswordAsync(user, request.ResetPasswordToken, request.Password);
            if (!result.Succeeded) return new ErrorResult(Messages.PasswordResetFailed);
            user.LastModifiedDate = DateTime.Now;
            user.LastModifiedBy = currentUser.UserName;
            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded && result.Succeeded)
            {
                return new SuccessResult(Messages.PasswordHasBeenResetSuccessfully);
            }

            return new ErrorResult(Messages.PasswordResetFailed);
        }
    }
}