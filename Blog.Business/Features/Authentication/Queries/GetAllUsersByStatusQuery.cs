using System.Collections.Generic;
using Blog.Core.Entities.DTOs.Authentication.Responses;
using Blog.Core.Utilities.Results;
using MediatR;

namespace Blog.Business.Features.Authentication.Queries
{
    public class GetAllUsersByStatusQuery : IRequest<IDataResult<List<UserResponse>>>
    {
        public bool Status { get; set; }
    }
}