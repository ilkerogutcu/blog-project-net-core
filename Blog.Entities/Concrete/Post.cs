using System;
using System.Collections.Generic;
using Blog.Core.Entities;

namespace Blog.Entities.Concrete
{
    public class Post:IEntity
    {
        public Guid Id { get; init; }
        public User User { get; set; }
        public Image Image { get; set; }
        public SeoDetail SeoDetail { get; set; }
        public Category Category { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Status { get; set; }
        public int ViewsCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}