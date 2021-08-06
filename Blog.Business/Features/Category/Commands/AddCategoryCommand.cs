using Blog.Core.Utilities.Results;
using Blog.Entities.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Blog.Business.Features.Category.Commands
{
    public class AddCategoryCommand : IRequest<IDataResult<CategoryDto>>
    {
        /// <summary>
        ///     Image url
        /// </summary>
        public IFormFile File { get; set; }

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