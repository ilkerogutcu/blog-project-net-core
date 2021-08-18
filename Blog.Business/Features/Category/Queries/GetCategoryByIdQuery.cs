using Blog.Business.Features.Category.Commands;
using Blog.Core.Utilities.Results;
using MediatR;

namespace Blog.Business.Features.Category.Queries
{
    public class GetCategoryByIdQuery : IRequest<IDataResult<UpdateCategoryCommand>>
    {
        public string CategoryId { get; set; }
    }
}