using System.Collections.Generic;
using Blog.Core.Entities.Concrete;
using Blog.Core.Utilities.Results;
using Blog.Entities.DTOs;
using MediatR;

namespace Blog.Business.Features.Category.Queries
{
    public class GetAllActiveCategoriesQuery : IRequest<IDataResult<IEnumerable<CategoryDto>>>
    {
        public PaginationFilter PaginationFilter { get; set; }
        public string Route { get; set; }
    }
}