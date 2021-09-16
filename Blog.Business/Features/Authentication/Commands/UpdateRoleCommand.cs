using Blog.Core.Utilities.Results;
using Blog.Entities.DTOs;
using MediatR;

namespace Blog.Business.Features.Authentication.Commands
{
    public class UpdateRoleCommand : IRequest<IDataResult<RoleDto>>
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}