using System;
using System.Threading.Tasks;
using Blog.Core.DataAccess.ElasticSearch;
using Blog.Core.DataAccess.ElasticSearch.Models;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Blog.Consumer.Consumers.Category
{
    public class CategoryUpdatedConsumer : IConsumer<Entities.Concrete.Category>
    {
        private readonly IElasticSearch _elasticSearch;
        private readonly ILogger<CategoryUpdatedConsumer> _log;

        public CategoryUpdatedConsumer(IElasticSearch elasticSearch, ILogger<CategoryUpdatedConsumer> log)
        {
            _elasticSearch = elasticSearch;
            _log = log;
        }

        public async Task Consume(ConsumeContext<Entities.Concrete.Category> context)
        {
            try
            {
                var category = context.Message;

                if (category == null) return;
                _log.LogInformation(
                    $"{category.Name} updated by {category.User.FirstName} - {category.User.LastName} ");

                var categoryElastic = await _elasticSearch.GetSearchByField<Entities.Concrete.Category>(
                    new SearchByFieldParameters
                    {
                        FieldName = "id",
                        IndexName = "category",
                        Value = category.Id.ToString()
                    });
                var result = await _elasticSearch.UpdateByElasticIdAsync(
                    new ElasticSearchInsertUpdateModel
                    {
                        Item = category,
                        ElasticId = categoryElastic[0].ElasticId,
                        IndexName = "category"
                    });
                if (!result.Success) return;
                _log.LogTrace($"Category -- Category name: {category.Name} " +
                              $"Category description: {category.Description} " +
                              $"Created by: {category.User.UserName} " +
                              $"Updated date : {category.LastModifiedDate} " +
                              $"Updated by : {category.LastModifiedBy} " +
                              "updated successfully");
            }
            catch (Exception e)
            {
                _log.LogError($"Update category from elasticsearch failed. Error message {e}");
                throw;
            }
        }
    }
}