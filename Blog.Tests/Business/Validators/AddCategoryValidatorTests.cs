using System.IO;
using System.Text;
using System.Threading.Tasks;
using Blog.Business.Features.Category.Commands;
using Blog.Business.Features.Category.ValidationRules;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Blog.Tests.Business.Validators
{
    public class AddCategoryValidatorTests
    {
        private readonly AddCategoryValidator _addCategoryValidator;

        public AddCategoryValidatorTests()
        {
            _addCategoryValidator = new AddCategoryValidator();
        }

        [Fact]
        private async Task Should_have_not_error_when_Name_is_specified()
        {
            var category = new AddCategoryCommand
            {
                Name = "Test"
            };

            var result = await _addCategoryValidator.TestValidateAsync(category);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        private async Task Should_have_not_error_when_Description_is_specified()
        {
            var category = new AddCategoryCommand
            {
                Description = "Test"
            };

            var result = await _addCategoryValidator.TestValidateAsync(category);
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        private async Task Should_have_error_when_Name_is_null()
        {
            var category = new AddCategoryCommand
            {
                Description = "Test",
                File = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("dummy image")), 0, 5, "Data", "image.png")
            };

            var result = await _addCategoryValidator.TestValidateAsync(category);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        private async Task Should_have_error_when_Description_is_null()
        {
            var category = new AddCategoryCommand
            {
                Name = "Test",
                File = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("dummy image")), 0, 5, "Data", "image.png")
            };

            var result = await _addCategoryValidator.TestValidateAsync(category);
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }
    }
}