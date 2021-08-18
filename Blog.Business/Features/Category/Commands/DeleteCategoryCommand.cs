using Blog.Core.Utilities.Results;
using MediatR;

namespace Blog.Business.Features.Category.Commands
{
    public class DeleteCategoryCommand : IRequest<IResult>
    {
        public string CategoryName { get; set; }
    }
}