using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Business.Abstract;
using Blog.Business.Constants;
using Blog.Business.Features.Authentication.Commands;
using Blog.Business.Features.Authentication.ValidationRules;
using Blog.Business.Helpers;
using Blog.Core.Aspects.Autofac.Exception;
using Blog.Core.Aspects.Autofac.Logger;
using Blog.Core.Aspects.Autofac.Validation;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Blog.Core.Entities.DTOs.Authentication.Responses;
using Blog.Core.Utilities.IoC;
using Blog.Core.Utilities.Results;
using Blog.Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Business.Features.Authentication.Handlers.Commands
{
    /// <summary>
    /// Sign in
    /// </summary>
    public class SignInCommandHandler : IRequestHandler<SignInCommand, IDataResult<SignInResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationMailService _authenticationMailService;

        public SignInCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager,
            IAuthenticationMailService authenticationMailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
            _authenticationMailService = authenticationMailService;
        }

        [ExceptionLogAspect(typeof(FileLogger))]
        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(SignUpValidator))]
        public async Task<IDataResult<SignInResponse>> Handle(SignInCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user is null)
            {
                return new ErrorDataResult<SignInResponse>(Messages.UserNotFound);
            }

            if (!user.EmailConfirmed)
            {
                return new ErrorDataResult<SignInResponse>(Messages.EmailIsNotConfirmed);
            }

            await _signInManager.SignOutAsync();
            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password,
                request.RememberMe, true);
            if (result.RequiresTwoFactor)
            {
                var code = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                await _authenticationMailService.SendTwoFactorCodeEmail(user, code);
                return new SuccessDataResult<SignInResponse>(Messages.Sent2FaCodeEmailSuccessfully);
            }

            if (result.IsLockedOut)
            {
                var remainingTime = (user.LockoutEnd.Value - DateTimeOffset.Now);
                return new ErrorDataResult<SignInResponse>(
                    $"Your account is locked out. Please wait for {remainingTime.Minutes}:{remainingTime.Seconds} and try again");
            }

            if (!result.Succeeded)
            {
                return new ErrorDataResult<SignInResponse>(Messages.SignInFailed);
            }

            var token = await AuthenticationHelper.GenerateJwtToken(user, _configuration, _userManager);
            var userRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            user.LastLoginDate = DateTime.Now;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return new ErrorDataResult<SignInResponse>(Messages.FailedToUpdateUser);
            }

            return new SuccessDataResult<SignInResponse>(new SignInResponse
            {
                Id = user.Id,
                Email = user.Email,
                Roles = userRoles.ToList(),
                Username = user.UserName,
                TwoStepIsEnabled = user.TwoFactorEnabled,
                IsVerified = user.EmailConfirmed,
                JwtToken = new JwtSecurityTokenHandler().WriteToken(token)
            }, Messages.SignInSuccessfully);
        }
    }
}