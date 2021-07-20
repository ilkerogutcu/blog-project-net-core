using System;
using System.Collections.Generic;
using Blog.Core.Entities;
using Blog.Entities.Concrete;

namespace Blog.Entities.DTOs
{
    public class PostDto:IDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        public SeoDetail SeoDetail { get; set; }
        public int ViewsCount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Tag> Tags { get; set; }
    }
}