using System;

namespace Blog.Entities.Concrete
{
    public class Image
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}