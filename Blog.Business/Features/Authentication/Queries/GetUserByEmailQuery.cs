using Blog.Core.Entities.DTOs.Authentication.Responses;
using Blog.Core.Utilities.Results;
using MediatR;

namespace Blog.Business.Features.Authentication.Queries
{
	/// <summary>
	///     Get user by email
	/// </summary>
	public class GetUserByEmailQuery : IRequest<IDataResult<UserResponse>>
	{
		public string Email { get; set; }
	}
}