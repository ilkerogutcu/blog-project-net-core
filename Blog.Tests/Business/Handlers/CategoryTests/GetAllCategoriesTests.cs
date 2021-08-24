using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Business.Abstract;
using Blog.Business.Constants;
using Blog.Business.Features.Category.Handlers.Queries;
using Blog.Business.Features.Category.Queries;
using Blog.Core.DataAccess.ElasticSearch;
using Blog.Core.DataAccess.ElasticSearch.Models;
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
    public class GetAllCategoriesTests
    {
     
        private readonly Mock<IElasticSearch> _elasticSearch;



        public GetAllCategoriesTests()
        {
            _elasticSearch = new Mock<IElasticSearch>();
        }

        private static Category CreateCategory(bool status)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Description = "Category test",
                Image = new Image(),
                Name = "Category Test",
                Status = status,
                User = new User(),
                CreatedDate = DateTime.Now
            };
        }

        [Fact]
        private async Task GetAllCategories_WithExistingCategories_ReturnsSuccessResult()
        {
            // Arrange
            var query = new GetAllCategoriesQuery();

            _elasticSearch.Setup(x => x.GetAllSearch<Category>(It.IsAny<SearchParameters>()))
                .ReturnsAsync(new List<ElasticSearchGetModel<Category>>
                {
                    new()
                    {
                        Item = CreateCategory(true),
                        ElasticId = Guid.NewGuid().ToString()
                    },
                    new()
                    {
                        Item = CreateCategory(true),
                        ElasticId = Guid.NewGuid().ToString()
                    }
                }).Verifiable();
            ;
            var handler = new GetAllCategoriesQueryHandler(_elasticSearch.Object, MockHelper.CreateMapper());

            // Act
            var result = await handler.Handle(query, new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Count().Should().BeGreaterThan(1);
        }

        [Fact]
        private async Task GetAllCategories_WithUnexistingCategories_ReturnsErrorResult()
        {
            // Arrange
            var query = new GetAllCategoriesQuery();

            _elasticSearch.Setup(x => x.GetAllSearch<Category>(It.IsAny<SearchParameters>()))
                .ReturnsAsync(() => null).Verifiable();
            ;
            var handler = new GetAllCategoriesQueryHandler(_elasticSearch.Object, MockHelper.CreateMapper());

            // Act
            var result = await handler.Handle(query, new CancellationToken());

            // Assert
            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Message.Should().Be(Messages.DataNotFound);
        }

        [Fact]
        private async Task GetAllActiveCategories_WithExistingCategories_ReturnsSuccessResult()
        {
            // Arrange
            var query = new GetAllActiveCategoriesQuery();

            _elasticSearch.Setup(x => x.GetSearchByField<Category>(It.IsAny<SearchByFieldParameters>()))
                .ReturnsAsync(new List<ElasticSearchGetModel<Category>>
                {
                    new()
                    {
                        Item = CreateCategory(true),
                        ElasticId = Guid.NewGuid().ToString()
                    },
                    new()
                    {
                        Item = CreateCategory(true),
                        ElasticId = Guid.NewGuid().ToString()
                    }
                }).Verifiable();
            ;
            var handler = new GetAllActiveCategoriesQueryHandler(_elasticSearch.Object, MockHelper.CreateMapper());

            // Act
            var result = await handler.Handle(query, new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Count().Should().BeGreaterThan(1);
        }

        [Fact]
        private async Task GetAllActiveCategories_WithUnexistingCategories_ReturnsErrorResult()
        {
            // Arrange
            var query = new GetAllActiveCategoriesQuery();

            _elasticSearch.Setup(x => x.GetSearchByField<Category>(It.IsAny<SearchByFieldParameters>()))
                .ReturnsAsync(() => null).Verifiable();
            ;
            var handler = new GetAllActiveCategoriesQueryHandler(_elasticSearch.Object, MockHelper.CreateMapper());

            // Act
            var result = await handler.Handle(query, new CancellationToken());

            // Assert
            result.Message.Should().Be(Messages.DataNotFound);
            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();
        }

        [Fact]
        private async Task GetAllNotActiveCategories_WithExistingCategories_ReturnsSuccessResult()
        {
            // Arrange
            var query = new GetAllNotActiveCategoriesQuery();

            _elasticSearch.Setup(x => x.GetSearchByField<Category>(It.IsAny<SearchByFieldParameters>()))
                .ReturnsAsync(new List<ElasticSearchGetModel<Category>>
                {
                    new()
                    {
                        Item = CreateCategory(false),
                        ElasticId = Guid.NewGuid().ToString()
                    },
                    new()
                    {
                        Item = CreateCategory(false),
                        ElasticId = Guid.NewGuid().ToString()
                    }
                }).Verifiable();
            var handler = new GetAllNotActiveCategoriesQueryHandler(_elasticSearch.Object, MockHelper.CreateMapper());

            // Act
            var result = await handler.Handle(query, new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Count().Should().BeGreaterThan(1);
        }

        [Fact]
        private async Task GetAllNotCategories_WithUnexistingCategories_ReturnsErrorResult()
        {
            // Arrange
            var query = new GetAllNotActiveCategoriesQuery();

            _elasticSearch.Setup(x => x.GetSearchByField<Category>(It.IsAny<SearchByFieldParameters>()))
                .ReturnsAsync(() => null).Verifiable();

            var handler = new GetAllNotActiveCategoriesQueryHandler(_elasticSearch.Object, MockHelper.CreateMapper());

            // Act
            var result = await handler.Handle(query, new CancellationToken());

            // Assert
            result.Message.Should().Be(Messages.DataNotFound);
            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();
        }
    }
}