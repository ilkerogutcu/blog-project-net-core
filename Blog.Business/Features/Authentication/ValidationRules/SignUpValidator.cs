using Blog.Business.Constants;
using Blog.Core.Entities.DTOs.Authentication.Requests;
using FluentValidation;

namespace Blog.Business.Features.Authentication.ValidationRules
{
    /// <summary>
    ///     Validator for sign up
    /// </summary>
    public class SignUpValidator : AbstractValidator<SignUpRequest>
    {
        public SignUpValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(Messages.PleaseEnterTheEmail);
            RuleFor(x => x.Email).EmailAddress().WithMessage(Messages.PleaseEnterAValidEmail);
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(Messages.PleaseEnterTheFirstName);
            RuleFor(x => x.LastName).NotEmpty().WithMessage(Messages.PleaseEnterTheLastName);
            RuleFor(x => x.Password).NotEmpty().WithMessage(Messages.PleaseEnterThePassword);
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage(Messages.PleaseConfirmYourPassword);
            RuleFor(x => x.Password).Equal(x => x.ConfirmPassword).WithMessage(Messages.PasswordsDontMatch);
            RuleFor(x => x.Username).Matches(@"^[a-zA-Z0-9]+$").WithMessage(Messages.UsernameIsNotInTheCorrectFormat);
            RuleFor(x => x.Username).NotEmpty().WithMessage(Messages.UsernameCannotBeEmpty);
            RuleFor(x => x.Bio).NotEmpty().WithMessage(Messages.PleaseEnterYourBiography);
            RuleFor(x => x.Bio).MaximumLength(500).WithMessage(Messages.FieldMustBeLessThan500Characters);
            RuleFor(x => x.Image).NotNull().WithMessage(Messages.PleaseSelectTheImage);
            RuleFor(x => x.Role).NotEmpty().WithMessage(Messages.PleaseSelectTheRole);
        }
    }
}