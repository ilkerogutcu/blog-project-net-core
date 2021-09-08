using Blog.Business.Constants;
using Blog.Business.Features.Authentication.Commands;
using FluentValidation;

namespace Blog.Business.Features.Authentication.ValidationRules
{
    public class UpdateUserValidator  : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(Messages.PleaseEnterTheFirstName);
            RuleFor(x => x.LastName).NotEmpty().WithMessage(Messages.PleaseEnterTheLastName);
            RuleFor(x => x.Bio).NotEmpty().WithMessage(Messages.PleaseEnterYourBiography);
            RuleFor(x => x.Bio).MaximumLength(500).WithMessage(Messages.FieldMustBeLessThan500Characters);
        }
    }
}