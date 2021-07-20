using System;
using System.Collections.Generic;
using Blog.Core.Utilities.Results;
using Blog.Entities.Concrete;
using MediatR;

namespace Blog.Business.Features.Post.Commands
{
    public class UpdatePostCommand : IRequest<IResult>
    {
        public Guid PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CategoryName { get; set; }
        public bool Status { get; set; }
        public string ImageUrl { get; set; }
        public SeoDetail SeoDetail { get; set; }
        public List<string> Tags { get; set; } 
    }
}