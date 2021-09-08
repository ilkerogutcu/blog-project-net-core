using Blog.Core.Utilities.Results;
using Blog.Entities.DTOs;
using MediatR;

namespace Blog.Business.Features.Authentication.Queries
{
    public class GetRoleByNameQuery: IRequest<IDataResult<RoleDto>>
    {
        public string Name { get; set; }
    }
}