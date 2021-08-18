using System.Threading.Tasks;
using Blog.Core.DataAccess.ElasticSearch;
using Blog.Core.DataAccess.ElasticSearch.Models;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Blog.Consumer.Consumers.Category
{
    public class CategoryAddedConsumer : IConsumer<Entities.Concrete.Category>
    {
        private readonly IElasticSearch _elasticSearch;
        private readonly ILogger<CategoryAddedConsumer> _log;

        public CategoryAddedConsumer(IElasticSearch elasticSearch, ILogger<CategoryAddedConsumer> log)
        {
            _elasticSearch = elasticSearch;
            _log = log;
        }
        
        public async Task Consume(ConsumeContext<Entities.Concrete.Category> context)
        {
            var category = context.Message;
            if (category is null) return;
            _log.LogInformation(
                $"{category.User.FirstName} - {category.User.LastName} kişisi {category.Name} kategorisini ekledi");

            await _elasticSearch.CreateNewIndexAsync(new IndexModel
            {
                IndexName = "category",
                AliasName = "category_alias"
            });
            
            var result = await _elasticSearch.InsertAsync(new ElasticSearchInsertUpdateModel
            {
                Item = category,
                IndexName = "category"
            });

            if (!result.Success) return;
            
            _log.LogTrace($"Category -- Category name: {category.Name} " +
                          $"Category description: {category.Description} " +
                          $"Username: {category.User.UserName} " +
                          $"Updated date : {category.CreatedDate} " +
                          "added from elasticsearch successfully");
        }
    }
}