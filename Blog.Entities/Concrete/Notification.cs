using System;
using System.Collections.Generic;

namespace Blog.Entities.Concrete
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public bool Seen { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}