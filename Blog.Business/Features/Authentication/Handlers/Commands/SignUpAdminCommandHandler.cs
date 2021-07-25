using System;
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
using Blog.Core.Utilities.Results;
using Blog.Entities.Concrete;
using Blog.Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Core.Aspects.Autofac.Exception;

namespace Blog.Business.Features.Authentication.Handlers.Commands
{
	/// <summary>
	/// Sign up admin
	/// </summary>
	[TransactionScopeAspectAsync]
	public class SignUpAdminCommandHandler : IRequestHandler<SignUpAdminCommand, IDataResult<SignUpResponse>>
	{
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<User> _userManager;
		private readonly IAuthenticationMailService _authenticationMailService;
		private readonly IMapper _mapper;
		public SignUpAdminCommandHandler(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IAuthenticationMailService authenticationMailService, IMapper mapper)
		{
			_roleManager = roleManager;
			_userManager = userManager;
			_authenticationMailService = authenticationMailService;
			_mapper = mapper;
		}

		[ValidationAspect(typeof(SignUpValidator))]
		[LogAspect(typeof(FileLogger))]
		[ExceptionLogAspect(typeof(FileLogger))]
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
			var user = _mapper.Map<User>(request.SignUpRequest);
			user.CreatedDate=DateTime.Now;
			user.Photo = new Image
			{
				Url = request.SignUpRequest.ImageUrl,
			};
			user.Status = true;
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