using Blog.Core.DataAccess.EntityFramework;
using Blog.DataAccess.Abstract;
using Blog.DataAccess.Concrete.EntityFramework.Contexts;
using Blog.Entities.Concrete;

namespace Blog.DataAccess.Concrete.EntityFramework
{
    public class EfCategoryRepository : EfEntityRepositoryBase<Category, ApplicationDbContext>, ICategoryRepository
    {
        public EfCategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}