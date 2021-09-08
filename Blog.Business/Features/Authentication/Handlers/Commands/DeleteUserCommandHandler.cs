using System;
using System.Threading;
using System.Threading.Tasks;
using Blog.Business.Constants;
using Blog.Business.Features.Authentication.Commands;
using Blog.Core.Utilities.Results;
using Blog.Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Blog.Business.Features.Authentication.Handlers.Commands
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, IResult>
    {
        private readonly UserManager<User> _userManager;

        public DeleteUserCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user is null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }
            // var user = await _userManager.FindByEmailAsync(_httpContextAccessor?.HttpContext.User
            //     .FindFirst(ClaimTypes.Email)?.Value);

            var currentUser = await _userManager.FindByEmailAsync("ilkerogtc@gmail.com");

            user.Status = false;
            user.LastModifiedBy = currentUser.UserName;
            user.LastModifiedDate = DateTime.Now;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new SuccessResult(Messages.DeletedSuccessfully);
            }

            return new ErrorResult(Messages.DeleteFailed);
        }
    }
}