using Blog.Core.Utilities.Results;
using MediatR;

namespace Blog.Business.Features.Authentication.Commands
{
	public class SendEmailConfirmationTokenCommand : IRequest<IResult>
	{
		public string Username { get; set; }
	}
}