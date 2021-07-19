using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Core.DataAccess;
using Blog.Entities.Concrete;
using Blog.Entities.DTOs;

namespace Blog.DataAccess.Abstract
{
    public interface ICategoryRepository : IEntityRepository<Category>
    {
       Task<IEnumerable<CategoryDto>> GetAllAsync();
       Task<CategoryDto> GetByNameAsync(string name);
       Task<IEnumerable<CategoryDto>> GetAllByStatusAsync(bool status);

    }
}