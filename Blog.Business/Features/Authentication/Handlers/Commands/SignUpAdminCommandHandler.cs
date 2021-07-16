using System.Linq;
using Blog.Business.Abstract;
using Blog.Business.Constants;
using Blog.Business.Features.Authentication.Commands;
using Blog.Business.Features.Authentication.ValidationRules;
using Blog.Core.Aspects.Autofac.Logger;
using Blog.Core.Aspects.Autofac.Transaction;
using Blog.Core.Aspects.Autofac.Validation;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Blog.Core.Entities.DTOs.Authentication.Responses;
using Blog.Core.Utilities.Mail;
using Blog.Core.Utilities.Results;
using Blog.Entities.Concrete;
using Blog.Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Business.Features.Authentication.Handlers.Commands
{
	[TransactionScopeAspectAsync]
	public class SignUpAdminCommandHandler : IRequestHandler<SignUpAdminCommand, IDataResult<SignUpResponse>>
	{
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<User> _userManager;
		private IAuthenticationMailService _authenticationMailService;

		public SignUpAdminCommandHandler(IMailService mailService, RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IAuthenticationMailService authenticationMailService)
		{
			_roleManager = roleManager;
			_userManager = userManager;
			_authenticationMailService = authenticationMailService;
		}

		[ValidationAspect(typeof(SignUpValidator))]
		[LogAspect(typeof(FileLogger))]
		public async Task<IDataResult<SignUpResponse>> Handle(SignUpAdminCommand request, CancellationToken cancellationToken)
		{
			var isUserAlreadyExist = await _userManager.FindByNameAsync(request.SignUpRequest.Username);
			if (isUserAlreadyExist is not null)
			{
				return new ErrorDataResult<SignUpResponse>(Messages.UsernameAlreadyExist);
			}
			var isEmailAlreadyExist = await _userManager.FindByEmailAsync(request.SignUpRequest.Username);
			if (isEmailAlreadyExist is not null)
			{
				return new ErrorDataResult<SignUpResponse>(Messages.EmailAlreadyExist);
			}
			var user = new User
			{
				UserName = request.SignUpRequest.Username,
				Email = request.SignUpRequest.Email,
				FirstName = request.SignUpRequest.FirstName,
				LastName = request.SignUpRequest.LastName
			};
			var result = await _userManager.CreateAsync(user, request.SignUpRequest.Password);
			if (!result.Succeeded)
			{
				return new ErrorDataResult<SignUpResponse>(Messages.SignUpFailed +
														   $":{result.Errors.ToList()[0].Description}");
			}
			if (!await _roleManager.RoleExistsAsync(Roles.Admin.ToString()))
			{
				await _roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
			}
			await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
			var verificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			var verificationUri = await _authenticationMailService.SendVerificationEmail(user, verificationToken);
			return new SuccessDataResult<SignUpResponse>(new SignUpResponse
			{
				Id = user.Id,
				Email = user.Email,
				UserName = user.UserName
			}, Messages.SignUpSuccessfully + verificationUri);
		}
	}
}