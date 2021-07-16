using Blog.Core.Entities.DTOs.Authentication.Requests;
using Blog.Core.Entities.DTOs.Authentication.Responses;
using Blog.Core.Utilities.Results;
using MediatR;

namespace Blog.Business.Features.Authentication.Commands
{
	public class SignUpUserCommand : IRequest<IDataResult<SignUpResponse>>
	{
		public SignUpRequest SignUpRequest { get; set; }
	}
}