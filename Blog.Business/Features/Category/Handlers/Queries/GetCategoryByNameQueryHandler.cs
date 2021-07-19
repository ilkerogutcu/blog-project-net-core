using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Business.Constants;
using Blog.Business.Features.Category.Queries;
using Blog.Core.Utilities.Results;
using Blog.DataAccess.Abstract;
using Blog.Entities.DTOs;
using MediatR;

namespace Blog.Business.Features.Category.Handlers.Queries
{
    public class GetCategoryByNameQueryHandler : IRequestHandler<GetCategoryByNameQuery, IDataResult<CategoryDto>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByNameQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IDataResult<CategoryDto>> Handle(GetCategoryByNameQuery request, CancellationToken cancellationToken)
        {
            var result = await _categoryRepository.GetByNameAsync(request.CategoryName);
            if (result is null)
            {
                return new ErrorDataResult<CategoryDto>(Messages.DataNotFound);
            }

            return new SuccessDataResult<CategoryDto>(result);
        }
    }
}