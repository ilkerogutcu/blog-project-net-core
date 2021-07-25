using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blog.Core.DataAccess;
using Blog.Entities.Concrete;
using Blog.Entities.DTOs;

namespace Blog.DataAccess.Abstract
{
    public interface IPostRepository : IEntityRepository<Post>
    {
        Task<Post> GetWithTags(Expression<Func<Post, bool>> expression);
        Task<IEnumerable<PostDto>> GetAllWithTags();
        Task<IEnumerable<PostDto>> GetAllByStatusWithTags(bool status);
    }
}