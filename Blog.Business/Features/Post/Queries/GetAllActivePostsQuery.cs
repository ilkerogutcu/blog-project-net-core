using System.Collections.Generic;
using Blog.Core.Entities.Concrete;
using Blog.Core.Utilities.Results;
using Blog.Entities.DTOs;
using MediatR;

namespace Blog.Business.Features.Post.Queries
{
    public class GetAllActivePostsQuery : IRequest<IDataResult<IEnumerable<PostDto>>>
    {
        public PaginationFilter PaginationFilter { get; set; }
        public string Route { get; set; }
    }
}