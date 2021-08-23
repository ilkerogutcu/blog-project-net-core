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
    /// Get category by name
    /// </summary>
    public class GetCategoryByNameQueryHandler : IRequestHandler<GetCategoryByNameQuery, IDataResult<CategoryDto>>
    {
        private readonly IElasticSearch _elasticSearch;
        private readonly IMapper _mapper;

        public GetCategoryByNameQueryHandler(IElasticSearch elasticSearch, IMapper mapper)
        {
            _elasticSearch = elasticSearch;
            _mapper = mapper;
        }


        [ExceptionLogAspect(typeof(FileLogger))]
        public async Task<IDataResult<CategoryDto>> Handle(GetCategoryByNameQuery request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.CategoryName))
            {
                return new ErrorDataResult<CategoryDto>(Messages.ParameterCannotBeEmpty);
            }

            var category = await _elasticSearch.GetSearchByField<Entities.Concrete.Category>(
                new SearchByFieldParameters
                {
                    FieldName = "name",
                    IndexName = "category",
                    Value = request.CategoryName
                });

            if (category?[0].Item is null)
            {
                return new ErrorDataResult<CategoryDto>(Messages.DataNotFound);
            }

            var result = _mapper.Map<CategoryDto>(category[0].Item);
            return new SuccessDataResult<CategoryDto>(result);
        }
    }
}