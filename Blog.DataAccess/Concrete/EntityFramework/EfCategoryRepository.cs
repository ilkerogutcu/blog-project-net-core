using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.DataAccess.EntityFramework;
using Blog.DataAccess.Abstract;
using Blog.DataAccess.Concrete.EntityFramework.Contexts;
using Blog.Entities.Concrete;
using Blog.Entities.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Blog.DataAccess.Concrete.EntityFramework
{
    public class EfCategoryRepository : EfEntityRepositoryBase<Category, ApplicationDbContext>, ICategoryRepository
    {
        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var result = await (from category in Context.Categories
                join user in Context.Users on category.User.Id equals user.Id
                join image in Context.Images on category.Image.Id equals image.Id
                select new CategoryDto
                {
                    CategoryName = category.Name,
                    Description = category.Description,
                    Status = category.Status,
                    CreatedBy = user.UserName,
                    ImageUrl = image.Url,
                    CreatedDate = category.CreatedDate,
                    LastModifiedBy = category.LastModifiedBy,
                    LastModifiedDate = category.LastModifiedDate
                }).ToListAsync();
            return result;
        }

        public async Task<CategoryDto> GetByNameAsync(string name)
        {
            var result = await (from category in Context.Categories
                where category.Name.ToLower() == name.ToLower()
                join user in Context.Users on category.User.Id equals user.Id
                join image in Context.Images on category.Image.Id equals image.Id
                select new CategoryDto
                {
                    CategoryName = category.Name,
                    Description = category.Description,
                    Status = category.Status,
                    CreatedBy = user.UserName,
                    ImageUrl = image.Url,
                    CreatedDate = category.CreatedDate,
                    LastModifiedBy = category.LastModifiedBy,
                    LastModifiedDate = category.LastModifiedDate
                }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllByStatusAsync(bool status)
        {
            var result = await (from category in Context.Categories
                where category.Status == status
                join user in Context.Users on category.User.Id equals user.Id
                join image in Context.Images on category.Image.Id equals image.Id
                select new CategoryDto
                {
                    CategoryName = category.Name,
                    Description = category.Description,
                    Status = category.Status,
                    CreatedBy = user.UserName,
                    ImageUrl = image.Url,
                    CreatedDate = category.CreatedDate,
                    LastModifiedBy = category.LastModifiedBy,
                    LastModifiedDate = category.LastModifiedDate
                }).ToListAsync();
            return result;
        }

        public EfCategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}