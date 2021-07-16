using Blog.Core.Entities.DTOs.Authentication.Responses;
using Blog.Core.Utilities.Results;
using MediatR;

namespace Blog.Business.Features.Authentication.Queries
{
	public class SignInWithTwoFactorQuery : IRequest<IDataResult<SignInResponse>>
	{
		public string Code { get; set; }
	}
}