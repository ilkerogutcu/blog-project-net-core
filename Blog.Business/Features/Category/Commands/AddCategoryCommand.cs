using Blog.Core.Utilities.Results;
using MediatR;

namespace Blog.Business.Features.Category.Commands
{
    public class AddCategoryCommand : IRequest<IResult>
    {
        public string Username { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}