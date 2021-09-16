using System;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Blog.Business.Abstract;
using Blog.Business.Constants;
using Blog.Business.Features.Category.Commands;
using Blog.Business.Features.Category.Handlers.Commands;
using Blog.Core.Utilities.MessageBrokers.RabbitMq;
using Blog.DataAccess.Abstract;
using Blog.Entities.Concrete;
using Blog.Entities.DTOs;
using Blog.Tests.MockHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Blog.Tests.Business.Handlers.CategoryTests
{
    public class CreateCategoryTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepository;
        private readonly Mock<ICloudinaryService> _cloudinaryService;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly Mock<IRabbitMqProducer> _producer;

        public CreateCategoryTests()
        {
            _categoryRepository = new Mock<ICategoryRepository>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _cloudinaryService = new Mock<ICloudinaryService>();
            _producer = new Mock<IRabbitMqProducer>();
        }

        [Fact]
        private async Task CreateCategory_WithCategoryToCreate_ReturnsSuccessResult()
        {
            // Arrange
            var user = new User {Email = "Foo"};
            var userManager = MockHelper.MockUserManager<User>();

            var command = new AddCategoryCommand
            {
                Name = "Category test",
                Description = "Category test",
                File = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("dummy image")), 0, 5, "Data", "image.png")
            };
            _httpContextAccessor.Setup(x => x.HttpContext.User.FindFirst(It.IsAny<string>()))
                .Returns(() => null).Verifiable();
            ;

            _categoryRepository.Setup(x => x.AddAsync(It.IsAny<Category>())).ReturnsAsync(new Category()).Verifiable();
            _categoryRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1).Verifiable();

            userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user).Verifiable();

            var handler = new AddCategoryCommandHandler(_categoryRepository.Object, MockHelper.CreateMapper(),
                userManager.Object,
                _httpContextAccessor.Object, _cloudinaryService.Object, _producer.Object);

            // Act
            var result = await handler.Handle(command, new CancellationToken());

            // Assert
            result.Message.Should().Be(Messages.CategorySuccessfullyAdded);
            result.Success.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(command,
                cfg => cfg.ComparingByMembers<CategoryDto>().ExcludingMissingMembers());
        }

        [Fact]
        private async Task CreateCategory_WithCategoryToCreate_WhenSaveChangesFailed_ReturnsErrorResult()
        {
            // Arrange
            var user = new User {Email = "Foo"};
            var userManager = MockHelper.MockUserManager<User>();

            var command = new AddCategoryCommand
            {
                Name = "Category test",
                Description = "Category test",
                File = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("dummy image")), 0, 5, "Data", "image.png")
            };
            _httpContextAccessor.Setup(x => x.HttpContext.User.FindFirst(It.IsAny<string>()))
                .Returns(() => null).Verifiable();
            ;

            _categoryRepository.Setup(x => x.AddAsync(It.IsAny<Category>())).ReturnsAsync(new Category()).Verifiable();
            _categoryRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0).Verifiable();

            userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user).Verifiable();
            ;

            var handler = new AddCategoryCommandHandler(_categoryRepository.Object, MockHelper.CreateMapper(),
                userManager.Object,
                _httpContextAccessor.Object, _cloudinaryService.Object, _producer.Object);

            // Act
            var result = await handler.Handle(command, new CancellationToken());

            // Assert
            result.Message.Should().Be(Messages.AddCategoryFailed);
            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();
        }

        [Fact]
        private async Task CreateCategory_WithCategoryToCreate_WhenCategoryNameAlreadyExists_ReturnsErrorResult()
        {
            // Arrange
            var userManager = MockHelper.MockUserManager<User>();

            var command = new AddCategoryCommand
            {
                Name = "Category test",
                Description = "Category test",
                File = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("dummy image")), 0, 5, "Data", "image.png")
            };
            _httpContextAccessor.Setup(x => x.HttpContext.User.FindFirst(It.IsAny<string>()))
                .Returns(() => null).Verifiable();

            _categoryRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(new Category()).Verifiable();

            var handler = new AddCategoryCommandHandler(_categoryRepository.Object, MockHelper.CreateMapper(),
                userManager.Object,
                _httpContextAccessor.Object, _cloudinaryService.Object, _producer.Object);

            // Act
            var result = await handler.Handle(command, new CancellationToken());

            // Assert
            result.Message.Should().Be(Messages.CategoryAlreadyExist);
            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();
        }

        [Fact]
        private async Task CreateCategory_WithCategoryToCreate_WhenUserIsNotSignedIn_ReturnsErrorResult()
        {
            // Arrange
            var userManager = MockHelper.MockUserManager<User>();

            var command = new AddCategoryCommand
            {
                Name = "Category test",
                Description = "Category test",
                File = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("dummy image")), 0, 5, "Data", "image.png")
            };
            _httpContextAccessor.Setup(x => x.HttpContext.User.FindFirst(It.IsAny<string>())).Returns(() => null)
                .Verifiable();

            userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(() => null).Verifiable();

            var handler = new AddCategoryCommandHandler(_categoryRepository.Object, MockHelper.CreateMapper(),
                userManager.Object,
                _httpContextAccessor.Object, _cloudinaryService.Object, _producer.Object);

            // Act
            var result = await handler.Handle(command, new CancellationToken());

            // Assert
            result.Message.Should().Be(Messages.UserNotFound);
            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();
        }
    }
}