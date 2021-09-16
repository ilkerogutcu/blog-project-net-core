using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Business.Constants;
using Blog.Business.Features.Authentication.Queries;
using Blog.Core.Aspects.Autofac.Exception;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Blog.Core.Utilities.Results;
using Blog.Entities.DTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Business.Features.Authentication.Handlers.Queries
{
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, IDataResult<IEnumerable<RoleDto>>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        
        public GetAllRolesQueryHandler(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [ExceptionLogAspect(typeof(FileLogger))]

        public async Task<IDataResult<IEnumerable<RoleDto>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles =await _roleManager.Roles.ToListAsync(cancellationToken: cancellationToken);
            if (!roles.Any()) return new ErrorDataResult<IEnumerable<RoleDto>>(Messages.DataNotFound);
            var result = _mapper.Map<List<RoleDto>>(roles);
            return new SuccessDataResult<IEnumerable<RoleDto>>(result);
        }
    }
}