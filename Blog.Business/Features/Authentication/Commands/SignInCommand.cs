using Blog.Core.Entities.DTOs.Authentication.Responses;
using Blog.Core.Utilities.Results;
using MediatR;

namespace Blog.Business.Features.Authentication.Commands
{
    public class SignInCommand : IRequest<IDataResult<SignInResponse>>
    {
        /// <summary>
        ///     Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     Password
        /// </summary>
        public string Password { get; set; }


        /// <summary>
        /// Remember me
        /// </summary>
        public bool RememberMe { get; set; }
    }
}