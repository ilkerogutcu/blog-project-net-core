using Blog.Business.Constants;
using Blog.Business.Features.Tag.Commands;
using FluentValidation;

namespace Blog.Business.Features.Tag.ValidationRules
{
    public class CreateTagValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagValidator()
        {
            RuleFor(x => x.Name).MaximumLength(70).WithMessage("Name: " + Messages.FieldMustBeLessThan70Characters);
        }
    }
    public class UpdateTagValidator : AbstractValidator<UpdateTagCommand>
    {
        public UpdateTagValidator()
        {
            RuleFor(x => x.Name).MaximumLength(70).WithMessage("Name: " + Messages.FieldMustBeLessThan70Characters);
        }
    }
}