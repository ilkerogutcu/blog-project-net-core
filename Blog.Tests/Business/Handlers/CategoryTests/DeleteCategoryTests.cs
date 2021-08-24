using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Blog.Business.Abstract;
using Blog.Business.Constants;
using Blog.Business.Features.Category.Commands;
using Blog.Business.Features.Category.Handlers.Commands;
using Blog.Core.Utilities.MessageBrokers.RabbitMq;
using Blog.DataAccess.Abstract;
using Blog.Entities.Concrete;
using Blog.Tests.MockHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Blog.Tests.Business.Handlers.CategoryTests
{
    public class DeleteCategoryTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepository;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly Mock<IRabbitMqProducer> _producer;

        public DeleteCategoryTests()
        {
            _categoryRepository = new Mock<ICategoryRepository>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _producer = new Mock<IRabbitMqProducer>();
        }

        [Fact]
        private async Task DeleteCategory_WithExistingCategory_ReturnsSuccessResult()
        {
            // Arrange
            var user = new User {Email = "Foo"};
            var userManager = MockHelper.MockUserManager<User>();

            var command = new DeleteCategoryCommand
            {
                CategoryName = "Category test"
            };
            _httpContextAccessor.Setup(x => x.HttpContext.User.FindFirst(It.IsAny<string>()))
                .Returns(() => null).Verifiable();

            _categoryRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(new Category {Name = command.CategoryName, Status = true}).Verifiable();
            _categoryRepository.Setup(x => x.Update(It.IsAny<Category>()))
                .Returns(new Category()).Verifiable();
            _categoryRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1).Verifiable();

            userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user).Verifiable();

            var handler = new DeleteCategoryCommandHandler(_categoryRepository.Object, userManager.Object,
                _httpContextAccessor.Object, _producer.Object);

            // Act
            var result = await handler.Handle(command, new CancellationToken());

            // Assert
            result.Message.Should().Be(Messages.DeletedSuccessfully);
            result.Success.Should().BeTrue();
        }

        [Fact]
        private async Task DeleteCategory_WithUnexistingCategory_ReturnsErrorResult()
        {
            // Arrange
            var user = new User {Email = "Foo"};
            var userManager = MockHelper.MockUserManager<User>();

            var command = new DeleteCategoryCommand
            {
                CategoryName = "Category test"
            };
            _httpContextAccessor.Setup(x => x.HttpContext.User.FindFirst(It.IsAny<string>()))
                .Returns(() => null).Verifiable();

            _categoryRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(() => null).Verifiable();

            userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user).Verifiable();

            var handler = new DeleteCategoryCommandHandler(_categoryRepository.Object, userManager.Object,
                _httpContextAccessor.Object, _producer.Object);

            // Act
            var result = await handler.Handle(command, new CancellationToken());

            // Assert
            result.Message.Should().Be(Messages.DataNotFound);
            result.Success.Should().BeFalse();
        }

        [Fact]
        private async Task DeleteCategory_WithExistingCategory_WhenSaveChangesFailed_ReturnsErrorResult()
        {
            // Arrange
            var user = new User {Email = "Foo"};
            var userManager = MockHelper.MockUserManager<User>();

            var command = new DeleteCategoryCommand
            {
                CategoryName = "Category test"
            };
            _httpContextAccessor.Setup(x => x.HttpContext.User.FindFirst(It.IsAny<string>()))
                .Returns(() => null).Verifiable();

            _categoryRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(new Category {Name = command.CategoryName, Status = true}).Verifiable();
            _categoryRepository.Setup(x => x.Update(It.IsAny<Category>()))
                .Returns(new Category()).Verifiable();
            _categoryRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0).Verifiable();

            userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user).Verifiable();

            var handler = new DeleteCategoryCommandHandler(_categoryRepository.Object, userManager.Object,
                _httpContextAccessor.Object, _producer.Object);

            // Act
            var result = await handler.Handle(command, new CancellationToken());

            // Assert
            result.Message.Should().Be(Messages.DeleteFailed);
            result.Success.Should().BeFalse();
        }

        [Fact]
        private async Task DeleteCategory_WithExistingCategory_WhenUserIsNotSignedIn_ReturnsErrorResult()
        {
            // Arrange
            var userManager = MockHelper.MockUserManager<User>();

            var command = new DeleteCategoryCommand
            {
                CategoryName = "Category test"
            };
            _httpContextAccessor.Setup(x => x.HttpContext.User.FindFirst(It.IsAny<string>()))
                .Returns(() => null).Verifiable();

            userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(() => null).Verifiable();

            var handler = new DeleteCategoryCommandHandler(_categoryRepository.Object, userManager.Object,
                _httpContextAccessor.Object, _producer.Object);

            // Act
            var result = await handler.Handle(command, new CancellationToken());

            // Assert
            result.Message.Should().Be(Messages.UserNotFound);
            result.Success.Should().BeFalse();
        }
    }
}