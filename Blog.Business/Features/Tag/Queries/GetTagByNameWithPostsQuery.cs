using System.Collections.Generic;
using Blog.Core.Entities.Concrete;
using Blog.Core.Utilities.Results;
using Blog.Entities.DTOs;
using MediatR;

namespace Blog.Business.Features.Tag.Queries
{
    public class GetTagByNameWithPostsQuery : IRequest<IDataResult<TagWithPostsDto>>
    {
        public PaginationFilter PaginationFilter { get; set; }
        public string Route { get; set; }
        public string Name { get; set; }
    }
}