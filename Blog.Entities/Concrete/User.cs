using System;
using System.Collections.Generic;
using Blog.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Blog.Entities.Concrete
{
    public class User : IdentityUser, IEntity
    {
        public Image Photo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public char Gender { get; set; }
        public bool Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}