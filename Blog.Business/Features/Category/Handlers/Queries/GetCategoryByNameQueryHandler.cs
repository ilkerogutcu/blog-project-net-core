using System.Threading;
using System.Threading.Tasks;
using Blog.Business.Constants;
using Blog.Business.Features.Category.Queries;
using Blog.Core.Aspects.Autofac.Exception;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Blog.Core.Utilities.Results;
using Blog.DataAccess.Abstract;
using Blog.Entities.DTOs;
using MediatR;

namespace Blog.Business.Features.Category.Handlers.Queries
{
    /// <summary>
    /// Get category by name
    /// </summary>
    public class GetCategoryByNameQueryHandler : IRequestHandler<GetCategoryByNameQuery, IDataResult<CategoryDto>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByNameQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [ExceptionLogAspect(typeof(FileLogger))]
        public async Task<IDataResult<CategoryDto>> Handle(GetCategoryByNameQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.CategoryName))
            {
                return new ErrorDataResult<CategoryDto>(Messages.DataNotFound);
            }
            var result = await _categoryRepository.GetByNameAsync(request.CategoryName);
            if (result is null)
            {
                return new ErrorDataResult<CategoryDto>(Messages.DataNotFound);
            }

            return new SuccessDataResult<CategoryDto>(result);
        }
    }
}