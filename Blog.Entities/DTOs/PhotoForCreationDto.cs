using Blog.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Blog.Entities.DTOs
{
    public class PhotoForCreationDto:IDto
    {
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string PublicId { get; set; }
    }
}