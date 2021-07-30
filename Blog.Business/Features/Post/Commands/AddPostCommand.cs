using System.Collections.Generic;
using Blog.Core.Utilities.Results;
using Blog.Entities.Concrete;
using MediatR;

namespace Blog.Business.Features.Post.Commands
{
    public class AddPostCommand : IRequest<IResult>
    {
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
        ///     Image url
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        ///     Seo details
        ///     Seo Author
        ///     Seo Description
        ///     Seo Tags
        /// </summary>
        public SeoDetail SeoDetail { get; set; }

        /// <summary>
        ///     Tags
        /// </summary>
        public List<string> TagNames { get; set; }
    }
}