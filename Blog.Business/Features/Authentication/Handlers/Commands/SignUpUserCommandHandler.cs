﻿using Blog.Business.Constants;
using Blog.Business.Features.Authentication.Commands;
using Blog.Business.Features.Authentication.ValidationRules;
using Blog.Core.Aspects.Autofac.Logger;
using Blog.Core.Aspects.Autofac.Transaction;
using Blog.Core.Aspects.Autofac.Validation;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Blog.Core.Entities.DTOs.Authentication.Responses;
using Blog.Core.Entities.Mail;
using Blog.Core.Utilities.Mail;
using Blog.Core.Utilities.Results;
using Blog.Entities.Concrete;
using Blog.Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Business.Features.Authentication.Handlers.Commands
{
	/// <summary>
	///     Sign up for user
	/// </summary>
	[TransactionScopeAspectAsync]
	public class SignUpUserCommandHandler : IRequestHandler<SignUpUserCommand, IDataResult<SignUpResponse>>
	{
		private readonly IConfiguration _config;
		private readonly IMailService _mailService;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<User> _userManager;

		public SignUpUserCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
			IMailService mailService, IConfiguration config)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_mailService = mailService;
			_config = config;
		}

		/// <summary>
		///     Create a new user with user role
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		[ValidationAspect(typeof(SignUpValidator))]
		[LogAspect(typeof(FileLogger))]
		public async Task<IDataResult<SignUpResponse>> Handle(SignUpUserCommand request,
			CancellationToken cancellationToken)
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

			if (!await _roleManager.RoleExistsAsync(Roles.User.ToString()))
			{
				await _roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
			}

			await _userManager.AddToRoleAsync(user, Roles.User.ToString());
			var verificationUri = await SendVerificationEmail(user);
			return new SuccessDataResult<SignUpResponse>(new SignUpResponse
			{
				Id = user.Id,
				Email = user.Email,
				UserName = user.UserName
			}, Messages.SignUpSuccessfully + verificationUri);
		}

		/// <summary>
		///     Send verification email
		/// </summary>
		/// <param name="user"></param>
		/// <returns>Verification url</returns>
		private async Task<string> SendVerificationEmail(User user)
		{
			// Generate token for confirm email
			var verificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			verificationToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(verificationToken));

			// Generate endpoint url for verification url
			var endPointUrl = new Uri(string.Concat($"{_config.GetSection("BaseUrl").Value}", "api/account/confirm-email/"));
			var verificationUrl = QueryHelpers.AddQueryString(endPointUrl.ToString(), "userId", user.Id);

			// Edit forgot password email template for reset password link
			var emailTemplatePath = Path.Combine(Environment.CurrentDirectory,
				@"MailTemplates\SendVerificationEmailTemplate.html");
			using (var reader = new StreamReader(emailTemplatePath))
			{
				var mailTemplate = await reader.ReadToEndAsync();
				reader.Close();
				await _mailService.SendEmailAsync(new MailRequest
				{
					ToEmail = user.Email,
					Subject = "Please verification your email",
					Body = mailTemplate.Replace("[verificationUrl]", verificationUrl)
				});
			}

			return QueryHelpers.AddQueryString(verificationUrl, "verificationToken", verificationToken);
		}
	}
}