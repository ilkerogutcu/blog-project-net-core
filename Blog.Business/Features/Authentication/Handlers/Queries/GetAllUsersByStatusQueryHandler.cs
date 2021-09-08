using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Business.Constants;
using Blog.Business.Features.Authentication.Queries;
using Blog.Core.Entities.DTOs.Authentication.Responses;
using Blog.Core.Utilities.Results;
using Blog.Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Business.Features.Authentication.Handlers.Queries
{
    public class GetAllUsersByStatusQueryHandler : IRequestHandler<GetAllUsersByStatusQuery, IDataResult<List<UserResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public GetAllUsersByStatusQueryHandler(IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        } 

        public async Task<IDataResult<List<UserResponse>>> Handle(GetAllUsersByStatusQuery request,
            CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.Where(x => x.Status == request.Status).Include(x=>x.Photo).ToListAsync(cancellationToken);
            var result = _mapper.Map<List<UserResponse>>(users);
            return result.Any()
                ? new SuccessDataResult<List<UserResponse>>(result)
                : new ErrorDataResult<List<UserResponse>>(Messages.DataNotFound);
        }
    }
}