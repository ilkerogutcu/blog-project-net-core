using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Business.Constants;
using Blog.Business.Features.Category.Queries;
using Blog.Business.Helpers;
using Blog.Core.Aspects.Autofac.Exception;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Blog.Core.Utilities.Results;
using Blog.Core.Utilities.Uri;
using Blog.DataAccess.Abstract;
using Blog.Entities.DTOs;
using MediatR;

namespace Blog.Business.Features.Category.Handlers.Queries
{
    public class GetAllNotActiveCategoriesQueryHandler : IRequestHandler<GetAllNotActiveCategoriesQuery, IDataResult<IEnumerable<CategoryDto>>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUriService _uriService;
        
        public GetAllNotActiveCategoriesQueryHandler(ICategoryRepository categoryRepository, IUriService uriService)
        {
            _categoryRepository = categoryRepository;
            _uriService = uriService;
        }
        
        [ExceptionLogAspect(typeof(FileLogger))]
        public async Task<IDataResult<IEnumerable<CategoryDto>>> Handle(GetAllNotActiveCategoriesQuery request, CancellationToken cancellationToken)
        {
            var result = (await _categoryRepository.GetAllByStatusAsync(false)).ToList();

            return !result.Any()
                ?  new ErrorDataResult<List<CategoryDto>>(Messages.DataNotFound)
                : PaginationHelper.CreatePaginatedResponse(result, request.PaginationFilter, result.Count, _uriService, request.Route);
        }
    }
}