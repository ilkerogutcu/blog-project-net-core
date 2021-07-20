using Blog.Business.Constants;
using Blog.Business.Features.Post.Commands;
using FluentValidation;

namespace Blog.Business.Features.Post.ValidationRules
{
    public class AddPostValidator : AbstractValidator<AddPostCommand>
    {
        public AddPostValidator()
        {
            RuleFor(x => x.Title).MaximumLength(500).WithMessage("Title: " + Messages.FieldMustBeLessThan500Characters);
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title: " + Messages.PleaseEnterTheValue);
            RuleFor(x => x.Content).NotEmpty().WithMessage("Content: " + Messages.PleaseEnterTheValue);
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("CategoryName: " + Messages.PleaseEnterTheValue);
            RuleFor(x => x.SeoDetail).NotEmpty().WithMessage("SeoDetail: " + Messages.PleaseEnterTheValue);
        }
    }
    
    public class UpdatePostValidator : AbstractValidator<AddPostCommand>
    {
        public UpdatePostValidator()
        {
            RuleFor(x => x.Title).MaximumLength(500).WithMessage("Title: " + Messages.FieldMustBeLessThan500Characters);
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title: " + Messages.PleaseEnterTheValue);
            RuleFor(x => x.Content).NotEmpty().WithMessage("Content: " + Messages.PleaseEnterTheValue);
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("CategoryName: " + Messages.PleaseEnterTheValue);
            RuleFor(x => x.SeoDetail).NotEmpty().WithMessage("SeoDetail: " + Messages.PleaseEnterTheValue);
        }
    }
}