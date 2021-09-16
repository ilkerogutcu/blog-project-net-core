using Blog.Business.Constants;
using Blog.Business.Features.Authentication.Commands;
using FluentValidation;

namespace Blog.Business.Features.Authentication.ValidationRules
{
    public class SignInValidator : AbstractValidator<SignInCommand>
    {
        public SignInValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage(Messages.UsernameCannotBeEmpty);
            RuleFor(x => x.Password).NotEmpty().WithMessage(Messages.PleaseEnterThePassword);
        }
    }
}