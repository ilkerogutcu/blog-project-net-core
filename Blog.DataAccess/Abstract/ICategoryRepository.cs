using Blog.Core.DataAccess;
using Blog.Entities.Concrete;

namespace Blog.DataAccess.Abstract
{
    public interface ICategoryRepository : IEntityRepository<Category>
    {
        
    }
}