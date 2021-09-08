using Blog.Core.Utilities.Results;
using Blog.Entities.DTOs;
using MediatR;

namespace Blog.Business.Features.Authentication.Commands
{
    public class CreateRoleCommand : IRequest<IDataResult<RoleDto>>
    {
        public string Name { get; set; }
    }
}