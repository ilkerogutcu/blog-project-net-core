using System;
using System.Collections.Generic;
using Blog.Core.Utilities.Results;
using Blog.Entities.Concrete;
using MediatR;

namespace Blog.Business.Features.Post.Commands
{
    public class UpdatePostCommand : IRequest<IResult>
    {
        /// <summary>
        ///     Post id
        /// </summary>
        public Guid PostId { get; set; }

        /// <summary>
        ///     Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///     Category name
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        ///     Status
        ///     true is active
        ///     false is not active
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        ///     Image url
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        ///     Seo details
        /// </summary>
        public SeoDetail SeoDetail { get; set; }

        /// <summary>
        ///     Tags
        /// </summary>
        public List<string> Tags { get; set; }
    }
}