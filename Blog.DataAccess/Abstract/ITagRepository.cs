using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blog.Core.DataAccess;
using Blog.Entities.Concrete;

namespace Blog.DataAccess.Abstract
{
    public interface ITagRepository : IEntityRepository<Tag>
    {
        Task<List<Tag>> GetAllTagsWithPostsAsync();
        Task<Tag> GetTagWithPostsAsync(Expression<Func<Tag, bool>> expression);

    }
}