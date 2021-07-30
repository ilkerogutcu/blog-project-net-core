using System.Collections.Generic;
using Blog.Core.Entities.Concrete;
using Blog.Core.Utilities.Results;
using Blog.Entities.DTOs;
using MediatR;

namespace Blog.Business.Features.Tag.Queries
{
    public class GetAllTagsQuery : IRequest<IDataResult<IEnumerable<TagDto>>>
    {
        public PaginationFilter PaginationFilter { get; set; }
        public string Route { get; set; }
    }
}