using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Business.Constants;
using Blog.Business.Features.Authentication.Queries;
using Blog.Core.Utilities.Results;
using Blog.Entities.DTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Business.Features.Authentication.Handlers.Queries
{
    public class GetRoleByNameQueryHandler : IRequestHandler<GetRoleByNameQuery, IDataResult<RoleDto>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public GetRoleByNameQueryHandler(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IDataResult<RoleDto>> Handle(GetRoleByNameQuery request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Name == request.Name,
                cancellationToken: cancellationToken);
            if (role is null)
            {
                return new ErrorDataResult<RoleDto>(Messages.DataNotFound);
            }

            var roleDto = _mapper.Map<RoleDto>(role);
            return new SuccessDataResult<RoleDto>(roleDto);
        }
    }
}