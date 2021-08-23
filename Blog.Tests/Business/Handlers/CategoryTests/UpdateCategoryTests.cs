using System;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Business.Abstract;
using Blog.Business.Constants;
using Blog.Business.Features.Category.Commands;
using Blog.Business.Features.Category.Handlers.Commands;
using Blog.Business.Helpers;
using Blog.Core.Utilities.MessageBrokers.RabbitMq;
using Blog.DataAccess.Abstract;
using Blog.Entities.Concrete;
using Blog.Tests.MockHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using Xunit;

namespace Blog.Tests.Business.Handlers.CategoryTests
{
    public class UpdateCategoryTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepository;
        private readonly Mock<ICloudinaryService> _cloudinaryService;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly Mock<IRabbitMqProducer> _producer;

        public UpdateCategoryTests()
        {
            _categoryRepository = new Mock<ICategoryRepository>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _cloudinaryService = new Mock<ICloudinaryService>();
            _producer = new Mock<IRabbitMqProducer>();
        }

        private static IMapper CreateMapper()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperHelper()); //your AutoMapper helper
            });
            return mockMapper.CreateMapper();
        }

        [Fact]
        private async Task UpdateCategory_WithUnexistingCategory_ReturnsErrorResult()
        {
            // Arrange
            var user = new User {Email = "Foo"};
            var userManager = MockHelper.MockUserManager<User>();

            var command = new UpdateCategoryCommand
            {
                Id = Guid.NewGuid(),
                ImageUrl = "imageTestUrl",
                Name = "Category test",
                Description = "Category test",
                File = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("dummy image")), 0, 5, "Data", "image.png")
            };
            _httpContextAccessor.Setup(x => x.HttpContext.User.FindFirst(It.IsAny<string>()))
                .Returns(() => null).Verifiable();

            _categoryRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(() => null).Verifiable();

            userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user).Verifiable();

            var handler = new UpdateCategoryCommandHandler(_categoryRepository.Object, CreateMapper(),
                userManager.Object,
                _httpContextAccessor.Object, _producer.Object, _cloudinaryService.Object);

            // Act
            var result = await handler.Handle(command, new CancellationToken());

            // Assert
            result.Message.Should().Be(Messages.DataNotFound);
            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();
        }

        [Fact]
        private async Task UpdateCategory_WithExistingCategory_WhenSaveChangesFailed_ReturnsErrorResult()
        {
            // Arrange
            var user = new User {Email = "Foo"};
            var userManager = MockHelper.MockUserManager<User>();

            var command = new UpdateCategoryCommand
            {
                Id = Guid.NewGuid(),
                ImageUrl = "imageTestUrl",
                Name = "Category test",
                Description = "Category test",
                File = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("dummy image")), 0, 5, "Data", "image.png")
            };
            _httpContextAccessor.Setup(x => x.HttpContext.User.FindFirst(It.IsAny<string>()))
                .Returns(() => null).Verifiable();

            _categoryRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(new Category {Id = command.Id}).Verifiable();
            _categoryRepository.Setup(x => x.Update(It.IsAny<Category>()))
                .Returns(new Category()).Verifiable();
            _categoryRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0).Verifiable();

            userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user).Verifiable();

            var handler = new UpdateCategoryCommandHandler(_categoryRepository.Object, CreateMapper(),
                userManager.Object,
                _httpContextAccessor.Object, _producer.Object, _cloudinaryService.Object);

            // Act
            var result = await handler.Handle(command, new CancellationToken());

            // Assert
            result.Message.Should().Be(Messages.UpdateFailed);
            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();
        }

        [Fact]
        private async Task UpdateCategory_WithExistingCategory_WhenUserIsNotSignedIn_ReturnsErrorResult()
        {
            // Arrange
            var userManager = MockHelper.MockUserManager<User>();

            var command = new UpdateCategoryCommand
            {
                Id = Guid.NewGuid(),
                ImageUrl = "imageTestUrl",
                Name = "Category test",
                Description = "Category test",
                File = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("dummy image")), 0, 5, "Data", "image.png")
            };
            _httpContextAccessor.Setup(x => x.HttpContext.User.FindFirst(It.IsAny<string>()))
                .Returns(() => null).Verifiable();

            userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(() => null).Verifiable();

            var handler = new UpdateCategoryCommandHandler(_categoryRepository.Object, CreateMapper(),
                userManager.Object,
                _httpContextAccessor.Object, _producer.Object, _cloudinaryService.Object);

            // Act
            var result = await handler.Handle(command, new CancellationToken());

            // Assert
            result.Message.Should().Be(Messages.UserNotFound);
            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();
        }
    }
}