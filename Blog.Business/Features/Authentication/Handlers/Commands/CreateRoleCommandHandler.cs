using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Business.Constants;
using Blog.Business.Features.Authentication.Commands;
using Blog.Business.Features.Authentication.ValidationRules;
using Blog.Core.Aspects.Autofac.Logger;
using Blog.Core.Aspects.Autofac.Validation;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Blog.Core.Utilities.Results;
using Blog.Entities.DTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Blog.Business.Features.Authentication.Handlers.Commands
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, IDataResult<RoleDto>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public CreateRoleCommandHandler(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [LogAspect(typeof(FileLogger))]
        [ValidationAspect(typeof(CreateRoleValidator))]
        public async Task<IDataResult<RoleDto>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Name))
                return new ErrorDataResult<RoleDto>(Messages.ParameterCannotBeEmpty);

            if (await _roleManager.RoleExistsAsync(request.Name))
                return new ErrorDataResult<RoleDto>(Messages.DataAlreadyExist);

            var identityRole = new IdentityRole(request.Name);
            var roleDto = _mapper.Map<RoleDto>(identityRole);
            var result = await _roleManager.CreateAsync(new IdentityRole(request.Name));
            if (result.Succeeded) return new SuccessDataResult<RoleDto>(roleDto, Messages.DataAddedSuccessfully);
            return new ErrorDataResult<RoleDto>(Messages.AddFailed + $":{result.Errors.ToList()[0].Description}");
        }
    }
}