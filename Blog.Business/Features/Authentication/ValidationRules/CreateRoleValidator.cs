using Blog.Business.Constants;
using Blog.Business.Features.Authentication.Commands;
using FluentValidation;

namespace Blog.Business.Features.Authentication.ValidationRules
{
    public class CreateRoleValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(Messages.ParameterCannotBeEmpty);
            RuleFor(x => x.Name).NotNull().WithMessage(Messages.ParameterCannotBeEmpty);
        }
    }
}