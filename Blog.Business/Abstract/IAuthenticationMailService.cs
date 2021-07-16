using Blog.Entities.Concrete;
using System.Threading.Tasks;

namespace Blog.Business.Abstract
{
	public interface IAuthenticationMailService
	{
		Task<string> SendVerificationEmail(User user, string verificationToken);
		Task SendForgotPasswordEmail(User user, string resetToken);
		Task SendTwoFactorCodeEmail(User user, string code);
	}
}