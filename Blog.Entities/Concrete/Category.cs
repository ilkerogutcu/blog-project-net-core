using System;
using System.Collections.Generic;
using Blog.Core.Entities;

namespace Blog.Entities.Concrete
{
    public class Category:IEntity
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public Image Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}