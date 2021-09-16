using System;
using Blog.Core.Entities.DTOs.Authentication.Responses;
using Blog.Core.Utilities.Results;
using MediatR;

namespace Blog.Business.Features.Authentication.Queries
{
	/// <summary>
	///     Get user by id
	/// </summary>
	public class GetUserByIdQuery : IRequest<IDataResult<UserResponse>>
	{
		/// <summary>
		/// User id
		/// </summary>
		public string Id { get; set; }
	}
}