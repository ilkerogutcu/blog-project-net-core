using Blog.Core.Utilities.Results;
using MediatR;

namespace Blog.Business.Features.Authentication.Commands
{
	public class UpdateTwoFactorSecurityCommand : IRequest<IResult>
	{
		public string userId { get; set; }
		public bool IsEnable { get; set; }
	}
}