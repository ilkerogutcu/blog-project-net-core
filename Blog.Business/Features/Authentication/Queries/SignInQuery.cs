using Blog.Core.Entities.DTOs.Authentication.Responses;
using Blog.Core.Utilities.Results;
using MediatR;

namespace Blog.Business.Features.Authentication.Queries
{
	public class SignInQuery : IRequest<IDataResult<SignInResponse>>
	{
		/// <summary>
		///     Username
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		///     Password
		/// </summary>
		public string Password { get; set; }
	}
}