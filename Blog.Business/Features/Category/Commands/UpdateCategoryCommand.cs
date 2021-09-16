using System;
using Blog.Core.Utilities.Results;
using Blog.Entities.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Blog.Business.Features.Category.Commands
{
    public class UpdateCategoryCommand : IRequest<IDataResult<CategoryDto>>
    {
        /// <summary>
        ///     Category id
        /// </summary>
        public Guid Id { get; set; }

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

        /// <summary>
        /// Category image
        /// </summary>
        public IFormFile File { get; set; }
    }
}