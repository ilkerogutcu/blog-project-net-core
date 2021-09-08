using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Business.Constants;
using Blog.Business.Features.Authentication.Commands;
using Blog.Business.Features.Authentication.ValidationRules;
using Blog.Core.Aspects.Autofac.Validation;
using Blog.Core.Utilities.Results;
using Blog.Entities.DTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Blog.Business.Features.Authentication.Handlers.Commands
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, IDataResult<RoleDto>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UpdateRoleCommandHandler(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [ValidationAspect(typeof(UpdateRoleValidator))]
        public async Task<IDataResult<RoleDto>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            if (await _roleManager.RoleExistsAsync(request.Name))
            {
                return new ErrorDataResult<RoleDto>(Messages.DataAlreadyExist);
            }

            var role = await _roleManager.FindByIdAsync(request.Id);
            role.Name = request.Name;
            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                return new ErrorDataResult<RoleDto>(Messages.UpdateFailed +
                                                    $":{result.Errors.ToList()[0].Description}");
            }

            var roleDto = _mapper.Map<RoleDto>(role);
            return new SuccessDataResult<RoleDto>(roleDto, Messages.DataAddedSuccessfully);
        }
    }
}