using System;
using Blog.Core.Entities;

namespace Blog.Entities.DTOs
{
    public class CategoryDto:IDto
    {
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
    }
}