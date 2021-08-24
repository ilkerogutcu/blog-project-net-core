using System;
using Blog.Core.Entities;

namespace Blog.Entities.Concrete
{
    public class Image:IEntity
    {
        public Guid Id { get; init; }
        public string Url { get; set; }
        public string PublicId { get; set; }
        public DateTime CreatedDate { get; set; }=DateTime.Now;
    }
}