using System;

namespace Blog.Entities.Concrete
{
    public class SeoDetail
    {
        public Guid Id { get; init; }
        public string SeoAuthor { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTags  { get; set; }
    }
}