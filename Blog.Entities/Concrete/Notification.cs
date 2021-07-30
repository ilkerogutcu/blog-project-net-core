using System;
using System.Collections.Generic;
using Blog.Core.Entities;

namespace Blog.Entities.Concrete
{
    public class Notification:IEntity
    {
        public Guid Id { get; init; }
        public string Message { get; set; }
        public bool Seen { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}