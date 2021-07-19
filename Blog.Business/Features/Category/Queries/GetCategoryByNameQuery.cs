using Blog.Core.Utilities.Results;
using Blog.Entities.DTOs;
using MediatR;

namespace Blog.Business.Features.Category.Queries
{
    public class GetCategoryByNameQuery : IRequest<IDataResult<CategoryDto>>
    {
        public string CategoryName { get; set; }
    }
}