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
    /// <summary>
    /// Get all categories
    /// </summary>
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IDataResult<IEnumerable<CategoryDto>>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        
        [ExceptionLogAspect(typeof(FileLogger))]
        public async Task<IDataResult<IEnumerable<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var result = (await _categoryRepository.GetAllAsync()).ToList();

            return !result.Any()
                ? new ErrorDataResult<List<CategoryDto>>(Messages.DataNotFound)
                : new SuccessDataResult<IEnumerable<CategoryDto>>(result);
        }
    }
}