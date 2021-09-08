using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.Entities.Mail;
using Blog.Core.Settings;
using Blog.Core.Utilities.IoC;
using MailKit.Net.Smtp;
using MailKit.Security;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Blog.Consumer.Consumers.Utils
{
    public class SendEmailConsumer : IConsumer<MailRequest>
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<SendEmailConsumer> _log;

        public SendEmailConsumer(ILogger<SendEmailConsumer> log)
        {
            _log = log;
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
            _mailSettings = configuration.GetSection("MailSettings").Get<MailSettings>();
        }

        public async Task Consume(ConsumeContext<MailRequest> context)
        {
            var mailRequest = context.Message;
            if (mailRequest != null)
            {
                var email = new MimeMessage
                {
                    Sender = MailboxAddress.Parse(_mailSettings.Mail),
                    Subject = mailRequest.Subject,
                    To = {MailboxAddress.Parse(mailRequest.ToEmail)}
                };

                var builder = new BodyBuilder();

                if (mailRequest.Attachments != null)
                {
                    foreach (var file in mailRequest.Attachments.Where(file => file.Length > 0))
                    {
                        byte[] fileBytes;
                        await using (var ms = new MemoryStream())
                        {
                            await file.CopyToAsync(ms);
                            fileBytes = ms.ToArray();
                        }

                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }

                builder.HtmlBody = mailRequest.Body;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
                _log.LogInformation($"Email: {mailRequest.ToEmail} Subject: {mailRequest.Subject} -- email sent");
                return;
            }

            _log.LogError("Mail request is null");
        }
    }
}