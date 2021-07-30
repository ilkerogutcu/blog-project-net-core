using System;
using Blog.Core.Entities;

namespace Blog.Entities.Concrete
{
    public class Comment:IEntity
    {
        public Guid Id { get; init; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
    }
}