using System;
using System.Collections.Generic;
using Blog.Core.Entities;

namespace Blog.Entities.Concrete
{
    public class Comment:IEntity
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}