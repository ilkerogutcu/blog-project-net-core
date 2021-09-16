using System.Collections.Generic;
using Blog.Core.Utilities.Results;
using Blog.Entities.DTOs;
using MediatR;

namespace Blog.Business.Features.Authentication.Queries
{
    public class GetAllRolesQuery: IRequest<IDataResult<IEnumerable<RoleDto>>>
    {
        
    }
}