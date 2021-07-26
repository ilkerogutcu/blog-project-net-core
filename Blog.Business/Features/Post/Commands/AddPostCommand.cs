using System.Collections.Generic;
using Blog.Core.Utilities.Results;
using Blog.Entities.Concrete;
using MediatR;

namespace Blog.Business.Features.Post.Commands
{
    public class AddPostCommand : IRequest<IResult>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        public SeoDetail SeoDetail { get; set; }
        public List<string> TagNames { get; set; }
    }
}