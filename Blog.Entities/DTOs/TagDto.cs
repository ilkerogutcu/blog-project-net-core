using System;
using Blog.Core.Entities;

namespace Blog.Entities.DTOs
{
    public class TagDto:IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}