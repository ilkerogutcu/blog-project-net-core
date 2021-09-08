using Blog.Core.Entities.DTOs.Authentication.Responses;
using Blog.Core.Utilities.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Blog.Business.Features.Authentication.Commands
{
    public class UpdateUserCommand : IRequest<IDataResult<UserResponse>>
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }
    }
}