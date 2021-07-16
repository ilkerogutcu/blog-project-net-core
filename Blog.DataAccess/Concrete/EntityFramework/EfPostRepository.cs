using Blog.Core.DataAccess.EntityFramework;
using Blog.DataAccess.Abstract;
using Blog.DataAccess.Concrete.EntityFramework.Contexts;
using Blog.Entities.Concrete;

namespace Blog.DataAccess.Concrete.EntityFramework
{
    public class EfPostRepository : EfEntityRepositoryBase<Post, ApplicationDbContext>, IPostRepository
    {
        public EfPostRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}