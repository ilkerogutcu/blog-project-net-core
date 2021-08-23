using System;
using System.Collections.Generic;
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
using Blog.Entities.DTOs;
using Blog.Tests.MockHelpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Blog.Tests.Business.Handlers.CategoryTests
{
    public class GetCategoryTests
    {
        private readonly Mock<IElasticSearch> _elasticSearch;


        public GetCategoryTests()
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
        private async Task GetCategoryById_WithExistingCategory_ReturnsSuccessResult()
        {
            // Arrange
            var query = new GetCategoryByIdQuery();
            var category = CreateCategory(true);
            _elasticSearch.Setup(x => x.GetSearchByField<Category>(It.IsAny<SearchByFieldParameters>()))
                .ReturnsAsync(new List<ElasticSearchGetModel<Category>>
                {
                    new()
                    {
                        Item = category,
                        ElasticId = Guid.NewGuid().ToString()
                    }
                }).Verifiable();
            
            var handler = new GetCategoryByIdQueryHandler(_elasticSearch.Object, MockHelper.CreateMapper());

            // Act
            var result = await handler.Handle(query, new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Data.Id.Should().Be(category.Id);
            result.Data.Should().BeEquivalentTo(category,
                cfg => cfg.ComparingByMembers<CategoryDto>().ExcludingMissingMembers());
        }

        [Fact]
        private async Task GetCategoryByName_WithExistingCategory_ReturnsSuccessResult()
        {
            // Arrange
            var category = CreateCategory(true);

            var query = new GetCategoryByNameQuery
            {
                CategoryName = category.Name
            };
            _elasticSearch.Setup(x => x.GetSearchByField<Category>(It.IsAny<SearchByFieldParameters>()))
                .ReturnsAsync(new List<ElasticSearchGetModel<Category>>
                {
                    new()
                    {
                        Item = category,
                        ElasticId = Guid.NewGuid().ToString()
                    }
                }).Verifiable();
            ;
            var handler = new GetCategoryByNameQueryHandler(_elasticSearch.Object, MockHelper.CreateMapper());

            // Act
            var result = await handler.Handle(query, new CancellationToken());

            // Assert
            result.Success.Should().BeTrue();
            result.Data.CategoryId.Should().Be(category.Id.ToString());
            result.Data.Should().BeEquivalentTo(category,
                cfg => cfg.ComparingByMembers<CategoryDto>().ExcludingMissingMembers());
        }

        [Fact]
        private async Task GetCategoryByName_WithUnexistingCategory_ReturnsErrorResult()
        {
            // Arrange
            var category = CreateCategory(true);

            var query = new GetCategoryByNameQuery
            {
                CategoryName = category.Name
            };
            _elasticSearch.Setup(x => x.GetSearchByField<Category>(It.IsAny<SearchByFieldParameters>()))
                .ReturnsAsync(() => null).Verifiable();
            ;
            var handler = new GetCategoryByNameQueryHandler(_elasticSearch.Object, MockHelper.CreateMapper());

            // Act
            var result = await handler.Handle(query, new CancellationToken());

            // Assert
            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Message.Should().Be(Messages.DataNotFound);
        }

        [Fact]
        private async Task GetCategoryByName_WhenCategoryNameIsEmpty_CannotBeEmpty_ReturnsErrorResult()
        {
            // Arrange
            var query = new GetCategoryByNameQuery
            {
                CategoryName = ""
            };
            _elasticSearch.Setup(x => x.GetSearchByField<Category>(It.IsAny<SearchByFieldParameters>()))
                .ReturnsAsync(() => null).Verifiable();

            var handler = new GetCategoryByNameQueryHandler(_elasticSearch.Object, MockHelper.CreateMapper());

            // Act
            var result = await handler.Handle(query, new CancellationToken());

            // Assert
            result.Success.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Message.Should().Be(Messages.ParameterCannotBeEmpty);
        }
    }
}