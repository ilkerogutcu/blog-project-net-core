using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blog.Core.DataAccess.EntityFramework;
using Blog.DataAccess.Abstract;
using Blog.DataAccess.Concrete.EntityFramework.Contexts;
using Blog.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace Blog.DataAccess.Concrete.EntityFramework
{
    public class EfPostRepository : EfEntityRepositoryBase<Post, ApplicationDbContext>, IPostRepository
    {
        public async Task<Post> GetWithTags(Expression<Func<Post, bool>> expression)
        {
            return await  Context.Posts.Include(x => x.Tags).AsQueryable().AsTracking().FirstOrDefaultAsync(expression);
        }

        public EfPostRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}