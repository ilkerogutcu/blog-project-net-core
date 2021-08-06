using Blog.Business.Constants;
using Blog.Business.Features.Category.Commands;
using FluentValidation;

namespace Blog.Business.Features.Category.ValidationRules
{
    public class AddCategoryValidator : AbstractValidator<AddCategoryCommand>
    {
        public AddCategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(Messages.PleaseEnterTheCategoryName);
            RuleFor(x => x.Description).NotEmpty().WithMessage(Messages.PleaseEnterTheDescription);
        }
    }
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(Messages.PleaseEnterTheCategoryName);
            RuleFor(x => x.Description).NotEmpty().WithMessage(Messages.PleaseEnterTheDescription);
        }
    }
}