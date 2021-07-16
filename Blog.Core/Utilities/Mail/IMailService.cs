using Blog.Core.Entities.Mail;
using System.Threading.Tasks;

namespace Blog.Core.Utilities.Mail
{
	public interface IMailService
	{
		Task SendEmailAsync(MailRequest mailRequest);
	}
}