using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Core.DataAccess.ElasticSearch.Models;
using Blog.Core.Utilities.Results;
using Nest;

namespace Blog.Core.DataAccess.ElasticSearch
{
    public interface IElasticSearch
    {
        Task<IResult> CreateNewIndexAsync(IndexModel indexModel);
        Task<IResult> InsertAsync(ElasticSearchInsertUpdateModel model);
        Task<IResult> InsertManyAsync(string indexName, object[] items);
        IReadOnlyDictionary<IndexName, IndexState> GetIndexList();

        Task<List<ElasticSearchGetModel<T>>> GetAllSearch<T>(SearchParameters parameters)
            where T : class;

        Task<List<ElasticSearchGetModel<T>>> GetSearchByField<T>(SearchByFieldParameters fieldParameters)
            where T : class;

        Task<long> GetCountAsync<T>(string indexName)  where T : class;

        Task<List<ElasticSearchGetModel<T>>> GetSearchBySimpleQueryString<T>(SearchByQueryParameters queryParameters)
            where T : class;

        Task<IResult> UpdateByElasticIdAsync(ElasticSearchInsertUpdateModel model);
        Task<IResult> DeleteByElasticIdAsync(ElasticSearchModel model);
    }
}