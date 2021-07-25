using Blog.Core.Utilities.Results;
using MediatR;

namespace Blog.Business.Features.Authentication.Commands
{
	public class UpdateTwoFactorSecurityCommand : IRequest<IResult>
	{
		/// <summary>
		/// User id
		/// </summary>
		public string UserId { get; set; }
		/// <summary>
		/// Enable two factor security; true or false
		/// </summary>
		public bool IsEnable { get; set; }
	}
}