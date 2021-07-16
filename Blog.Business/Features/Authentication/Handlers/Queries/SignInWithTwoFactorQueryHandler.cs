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

namespace Blog.Business.Features.Authentication.Handlers.Queries
{
	/// <summary>
	/// Sign in with two factor authentication
	/// </summary>
	public class SignInWithTwoFactorQueryHandler : IRequestHandler<SignInWithTwoFactorQuery, IDataResult<SignInResponse>>
	{
		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> _userManager;
		private readonly IConfiguration _configuration;

		public SignInWithTwoFactorQueryHandler(SignInManager<User> signInManager, UserManager<User> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
		}

		[LogAspect(typeof(FileLogger))]
		public async Task<IDataResult<SignInResponse>> Handle(SignInWithTwoFactorQuery request, CancellationToken cancellationToken)
		{
			var result = await _signInManager.TwoFactorSignInAsync("Email", request.Code, false, false);
			if (!result.Succeeded)
			{
				return new ErrorDataResult<SignInResponse>(Messages.SignInFailed);
			}

			var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
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