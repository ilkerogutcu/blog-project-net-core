using System;
using System.Collections.Generic;
using Blog.Core.Entities;

namespace Blog.Entities.Concrete
{
    public class Post:IEntity
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public Image Image { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Topic> Topics { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}