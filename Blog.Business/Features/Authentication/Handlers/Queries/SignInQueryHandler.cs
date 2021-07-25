using System;
using Blog.Business.Abstract;
using Blog.Business.Constants;
using Blog.Business.Features.Authentication.Queries;
using Blog.Business.Helpers;
using Blog.Core.Aspects.Autofac.Logger;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Blog.Core.Entities.DTOs.Authentication.Responses;
using Blog.Core.Utilities.IoC;
using Blog.Core.Utilities.Results;
using Blog.Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Aspects.Autofac.Exception;

namespace Blog.Business.Features.Authentication.Handlers.Queries
{
	/// <summary>
	/// Sign in
	/// </summary>
	public class SignInQueryHandler : IRequestHandler<SignInQuery, IDataResult<SignInResponse>>
	{
		private readonly IConfiguration _configuration;
		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> _userManager;
		private readonly IAuthenticationMailService _authenticationMailService;

		public SignInQueryHandler(UserManager<User> userManager, SignInManager<User> signInManager, IAuthenticationMailService authenticationMailService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>(); ;
			_authenticationMailService = authenticationMailService;
		}

		[ExceptionLogAspect(typeof(FileLogger))]
		[LogAspect(typeof(FileLogger))]
		public async Task<IDataResult<SignInResponse>> Handle(SignInQuery request, CancellationToken cancellationToken)
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
				false, true);
			if (result.RequiresTwoFactor)
			{
				var code = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
				await _authenticationMailService.SendTwoFactorCodeEmail(user, code);
				return new SuccessDataResult<SignInResponse>(Messages.Sent2FaCodeEmailSuccessfully);
			}
			if (result.IsLockedOut)
			{
				return new ErrorDataResult<SignInResponse>(Messages.YourAccountIsLockedOut);
			}

			if (!result.Succeeded)
			{
				return new ErrorDataResult<SignInResponse>(Messages.SignInFailed);
			}
			
			var token = await AuthenticationHelper.GenerateJwtToken(user, _configuration, _userManager);
			var userRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
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