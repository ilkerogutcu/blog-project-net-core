using System.Collections.Generic;
using Blog.Core.Entities.DTOs.Authentication.Responses;
using Blog.Core.Utilities.Results;
using MediatR;

namespace Blog.Business.Features.Authentication.Queries
{
    public class GetAllUsersQuery : IRequest<IDataResult<List<UserResponse>>>
    {
        
    }
}