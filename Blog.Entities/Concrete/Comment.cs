using System;
using System.Collections.Generic;

namespace Blog.Entities.Concrete
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}