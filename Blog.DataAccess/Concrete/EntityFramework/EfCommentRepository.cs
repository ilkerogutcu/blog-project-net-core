using Blog.Core.DataAccess.EntityFramework;
using Blog.DataAccess.Abstract;
using Blog.DataAccess.Concrete.EntityFramework.Contexts;
using Blog.Entities.Concrete;

namespace Blog.DataAccess.Concrete.EntityFramework
{
    public class EfCommentRepository : EfEntityRepositoryBase<Comment, ApplicationDbContext>, ICommentRepository
    {
        public EfCommentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}