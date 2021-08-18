using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Business.Constants;
using Blog.Business.Features.Category.Commands;
using Blog.Business.Features.Category.Queries;
using Blog.Core.DataAccess.ElasticSearch;
using Blog.Core.DataAccess.ElasticSearch.Models;
using Blog.Core.Utilities.Results;
using Blog.Entities.DTOs;
using MediatR;

namespace Blog.Business.Features.Category.Handlers.Queries
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, IDataResult<UpdateCategoryCommand>>
    {
        private readonly IElasticSearch _elasticSearch;
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryHandler(IElasticSearch elasticSearch, IMapper mapper)
        {
            _elasticSearch = elasticSearch;
            _mapper = mapper;
        }

        public async Task<IDataResult<UpdateCategoryCommand>> Handle(GetCategoryByIdQuery request,
            CancellationToken cancellationToken)
        {
            var category = (await _elasticSearch.GetSearchByField<Entities.Concrete.Category>(
                new SearchByFieldParameters
                {
                    FieldName = "id",
                    IndexName = "category",
                    Value = request.CategoryId
                }))[0].Item;
            
            if (category == null)
            {
                return new ErrorDataResult<UpdateCategoryCommand>(Messages.DataNotFound);
            }

            var updateCategory = _mapper.Map<UpdateCategoryCommand>(category);
            return new SuccessDataResult<UpdateCategoryCommand>(updateCategory);
        }
    }
}