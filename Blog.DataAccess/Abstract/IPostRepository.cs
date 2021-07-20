using System.Threading.Tasks;
using Blog.Core.DataAccess;
using Blog.Entities.Concrete;
using Blog.Entities.DTOs;

namespace Blog.DataAccess.Abstract
{
    public interface IPostRepository : IEntityRepository<Post>
    {
    }
}