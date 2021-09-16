using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Business.Abstract;
using Blog.Business.Constants;
using Blog.Business.Features.Authentication.Commands;
using Blog.Core.Entities.DTOs.Authentication.Responses;
using Blog.Core.Utilities.Results;
using Blog.Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Blog.Business.Features.Authentication.Handlers.Commands
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, IDataResult<UserResponse>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;
        
        public UpdateUserCommandHandler(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor,
            ICloudinaryService cloudinaryService, IMapper mapper)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
        }

        public async Task<IDataResult<UserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user is null)
            {
                return new ErrorDataResult<UserResponse>(Messages.UserNotFound);
            }
            // var user = await _userManager.FindByEmailAsync(_httpContextAccessor?.HttpContext.User
            //     .FindFirst(ClaimTypes.Email)?.Value);

            var currentUser = await _userManager.FindByEmailAsync("ilkerogtc@gmail.com");
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Bio = request.Bio;
            var file = request.Image;
            user.LastModifiedBy = currentUser.UserName;
            user.LastModifiedDate = DateTime.Now;
            if (request.Image is {Length: > 0})
            {
                user.Photo = await _cloudinaryService.UploadImage(file);
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new ErrorDataResult<UserResponse>(Messages.UpdateFailed);
            }

            var userResponse = _mapper.Map<UserResponse>(user);
            return new SuccessDataResult<UserResponse>(userResponse,Messages.UpdatedSuccessfully);

        }
    }
}