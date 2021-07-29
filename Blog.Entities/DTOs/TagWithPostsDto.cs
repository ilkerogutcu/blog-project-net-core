using System;
using System.Collections.Generic;
using Blog.Core.Entities;

namespace Blog.Entities.DTOs
{
    public class TagWithPostsDto:IDto
    {
        public Guid TagId { get; set; }
        public string TagName { get; set; }
        public List<PostDto> Posts { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}