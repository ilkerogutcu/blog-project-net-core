using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Business.Constants;
using Blog.Business.Features.Category.Queries;
using Blog.Core.Aspects.Autofac.Exception;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Blog.Core.DataAccess.ElasticSearch;
using Blog.Core.DataAccess.ElasticSearch.Models;
using Blog.Core.Utilities.Results;
using Blog.Entities.DTOs;
using MediatR;

namespace Blog.Business.Features.Category.Handlers.Queries
{
    /// <summary>
    ///     Get all categories
    /// </summary>
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IDataResult<IEnumerable<CategoryDto>>>
    {
        private readonly IElasticSearch _elasticSearch;
        private readonly IMapper _mapper;

        public GetAllCategoriesQueryHandler(IElasticSearch elasticSearch, IMapper mapper)
        {
            _elasticSearch = elasticSearch;
            _mapper = mapper;
        }

        [ExceptionLogAspect(typeof(FileLogger))]
        public async Task<IDataResult<IEnumerable<CategoryDto>>> Handle(GetAllCategoriesQuery request,
            CancellationToken cancellationToken)
        {
            var categoriesCount = await _elasticSearch.GetCountAsync<Entities.Concrete.Category>("category");
            var categories = await _elasticSearch.GetAllSearch<Entities.Concrete.Category>(new SearchParameters
            {
                IndexName = "category",
                Size = Convert.ToInt32(categoriesCount)
            });
            
            var result = _mapper.Map<List<CategoryDto>>(categories);
            return !result.Any()
                ? new ErrorDataResult<List<CategoryDto>>(Messages.DataNotFound)
                : new SuccessDataResult<IEnumerable<CategoryDto>>(result);
        }
    }
}