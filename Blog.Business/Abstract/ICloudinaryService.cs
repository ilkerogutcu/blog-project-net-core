using System.Threading.Tasks;
using Blog.Entities.Concrete;
using Microsoft.AspNetCore.Http;

namespace Blog.Business.Abstract
{
    public interface ICloudinaryService
    {
        Task<Image> UploadImage(IFormFile file);
    }
}