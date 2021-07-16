using Blog.Core.Entities.DTOs.Authentication.Responses;
using Blog.Core.Utilities.Results;
using MediatR;

namespace Blog.Business.Features.Authentication.Queries
{
	/// <summary>
	///     Get user by username
	/// </summary>
public class GetUserByUsernameQuery : IRequest<IDataResult<UserResponse>>
{
	public string Username { get; set; }
}
}