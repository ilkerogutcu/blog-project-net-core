using System;
using System.Collections.Generic;
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
    public class EfTagRepository : EfEntityRepositoryBase<Tag, ApplicationDbContext>, ITagRepository
    {
        public EfTagRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Tag>> GetAllTagsWithPostsAsync()
        {
            return await Context.Tags.Include(x => x.Posts).ToListAsync();
        }

        public async Task<Tag> GetTagWithPostsAsync(Expression<Func<Tag, bool>> expression)
        {
            return await Context.Tags
                .Include(x => x.Posts).ThenInclude(x=>x.Category)
                .Include(x => x.Posts).ThenInclude(x=>x.Tags)
                .Include(x => x.Posts).ThenInclude(x=>x.Image)
                .Include(x => x.Posts).ThenInclude(x=>x.SeoDetail)
                .Include(x=>x.User).AsTracking().FirstOrDefaultAsync(expression);
        }


        public async Task<List<Tag>> GetAllAsync(Expression<Func<Tag, bool>> expression = null)
        {
            return expression == null
                ? await Context.Tags.Include(x => x.User).AsTracking().ToListAsync()
                : await Context.Tags.Include(x => x.User).AsTracking().Where(expression).ToListAsync();
        }
    }
}