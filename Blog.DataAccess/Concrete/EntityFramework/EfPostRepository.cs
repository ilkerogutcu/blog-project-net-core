using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blog.Core.DataAccess.EntityFramework;
using Blog.DataAccess.Abstract;
using Blog.DataAccess.Concrete.EntityFramework.Contexts;
using Blog.Entities.Concrete;
using Blog.Entities.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Blog.DataAccess.Concrete.EntityFramework
{
    public class EfPostRepository : EfEntityRepositoryBase<Post, ApplicationDbContext>, IPostRepository
    {
        public EfPostRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Post> GetWithTags(Expression<Func<Post, bool>> expression)
        {
            return await Context.Posts.Include(x => x.Tags).AsQueryable().AsTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<IEnumerable<PostDto>> GetAllWithTags()
        {
            var result = await (from post in Context.Posts
                select new PostDto
                {
                    Id = post.Id,
                    Content = post.Content,
                    Status = post.Status,
                    Tags = post.Tags.Select(x => x.Name).ToList(),
                    Title = post.Title,
                    CategoryName = post.Category.Name,
                    CreatedBy = post.User.FirstName + " " + post.User.LastName,
                    CreatedDate = post.CreatedDate,
                    ImageUrl = post.Image.Url,
                    SeoDetail = post.SeoDetail,
                    ViewsCount = post.ViewsCount,
                    LastModifiedBy = post.LastModifiedBy,
                    LastModifiedDate = post.LastModifiedDate
                }).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<PostDto>> GetAllByStatusWithTags(bool status)
        {
            var result = await (from post in Context.Posts
                where post.Status == status
                select new PostDto
                {
                    Id = post.Id,
                    Content = post.Content,
                    Status = post.Status,
                    Tags = post.Tags.Select(x => x.Name).ToList(),
                    Title = post.Title,
                    CategoryName = post.Category.Name,
                    CreatedBy = post.User.FirstName + " " + post.User.LastName,
                    CreatedDate = post.CreatedDate,
                    ImageUrl = post.Image.Url,
                    SeoDetail = post.SeoDetail,
                    ViewsCount = post.ViewsCount,
                    LastModifiedBy = post.LastModifiedBy,
                    LastModifiedDate = post.LastModifiedDate
                }).ToListAsync();
            return result;
        }
    }
}