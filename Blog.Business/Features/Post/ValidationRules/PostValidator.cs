using Blog.Business.Constants;
using Blog.Business.Features.Category.Commands;
using FluentValidation;

namespace Blog.Business.Features.Post.ValidationRules
{
    public class PostValidator : AbstractValidator<AddCategoryCommand>
    {
        public PostValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(Messages.PleaseEnterTheCategoryName);
            RuleFor(x => x.ImageUrl).NotEmpty().WithMessage(Messages.PleaseEnterTheImageUrl);
            RuleFor(x => x.Description).NotEmpty().WithMessage(Messages.PleaseEnterTheDescription);
        }
    }
}