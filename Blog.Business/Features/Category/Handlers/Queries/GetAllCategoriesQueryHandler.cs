using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Business.Constants;
using Blog.Business.Features.Category.Queries;
using Blog.Business.Helpers;
using Blog.Core.Utilities.Results;
using Blog.Core.Utilities.Uri;
using Blog.DataAccess.Abstract;
using Blog.Entities.DTOs;
using MediatR;

namespace Blog.Business.Features.Category.Handlers.Queries
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IDataResult<IEnumerable<CategoryDto>>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUriService _uriService;

        public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository, IUriService uriService)
        {
            _categoryRepository = categoryRepository;
            _uriService = uriService;
        }

        public async Task<IDataResult<IEnumerable<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var result = (await _categoryRepository.GetAllAsync()).ToList();

            return !result.Any()
                ? (IDataResult<IEnumerable<CategoryDto>>) new ErrorDataResult<List<CategoryDto>>(Messages.DataNotFound)
                : PaginationHelper.CreatePaginatedResponse(result, request.PaginationFilter, result.Count, _uriService, request.Route);
        }
    }
}