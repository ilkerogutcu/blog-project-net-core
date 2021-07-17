using Blog.Core.DataAccess.EntityFramework;
using Blog.DataAccess.Abstract;
using Blog.DataAccess.Concrete.EntityFramework.Contexts;
using Blog.Entities.Concrete;

namespace Blog.DataAccess.Concrete.EntityFramework
{
    public class EfTagRepository : EfEntityRepositoryBase<Tag, ApplicationDbContext>, ITagRepository
    {
        public EfTagRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}