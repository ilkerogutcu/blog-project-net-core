﻿using Blog.Business.Constants;
using Blog.Business.Features.Authentication.Commands;
using FluentValidation;

namespace Blog.Business.Features.Authentication.ValidationRules
{
	/// <summary>
	///     Validator for sign up
	/// </summary>
	public class SignUpValidator : AbstractValidator<SignUpUserCommand>
	{
		public SignUpValidator()
		{
			RuleFor(x => x.SignUpRequest.Email).NotEmpty().WithMessage(Messages.PleaseEnterTheEmail);
			RuleFor(x => x.SignUpRequest.Email).EmailAddress().WithMessage(Messages.PleaseEnterAValidEmail);
			RuleFor(x => x.SignUpRequest.FirstName).NotEmpty().WithMessage(Messages.PleaseEnterTheFirstName);
			RuleFor(x => x.SignUpRequest.LastName).NotEmpty().WithMessage(Messages.PleaseEnterTheLastName);
			RuleFor(x => x.SignUpRequest.Password).Equal(x => x.SignUpRequest.ConfirmPassword).WithMessage(Messages.PasswordsDontMatch);
			RuleFor(x => x.SignUpRequest.Username).Matches(@"^[a-zA-Z-']*$").WithMessage(Messages.UsernameIsNotInTheCorrectFormat);
			RuleFor(x => x.SignUpRequest.Username).NotEmpty().WithMessage(Messages.UsernameCannotBeEmpty);
			RuleFor(x => x.SignUpRequest.Bio).MaximumLength(500).WithMessage(Messages.FieldMustBeLessThan500Characters);
		}
	}
}