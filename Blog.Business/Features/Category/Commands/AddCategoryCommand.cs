using Blog.Core.Utilities.Results;
using MediatR;

namespace Blog.Business.Features.Category.Commands
{
    public class AddCategoryCommand : IRequest<IResult>
    {
        /// <summary>
        ///     Image url
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        ///     Category name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Category description
        /// </summary>
        public string Description { get; set; }
    }
}