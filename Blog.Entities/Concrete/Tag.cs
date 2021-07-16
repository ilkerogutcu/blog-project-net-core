using System;
using System.Collections.Generic;
using Blog.Core.Entities;

namespace Blog.Entities.Concrete
{
    public class Tag:IEntity
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}