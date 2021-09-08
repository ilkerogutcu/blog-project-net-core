using Blog.Core.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;
using Blog.Core.Entities.Mail;
using Blog.Core.Settings;
using Blog.Core.Utilities.IoC;
using Blog.Core.Utilities.Messages;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.Utilities.MessageBrokers.RabbitMq;
using MassTransit.RabbitMqTransport;

namespace Blog.Core.Utilities.Mail
{
	public class MailService : IMailService
	{
		private readonly MailSettings _mailSettings;
		private readonly IRabbitMqProducer _producer;
		public MailService()
		{
			var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
			_mailSettings = configuration.GetSection("MailSettings").Get<MailSettings>();
			_producer= ServiceTool.ServiceProvider.GetService<IRabbitMqProducer>();
		}

		public async Task SendEmailAsync(MailRequest mailRequest)
		{
			var email = new MimeMessage
			{
				Sender = MailboxAddress.Parse(_mailSettings.Mail),
				Subject = mailRequest.Subject,
				To = { MailboxAddress.Parse(mailRequest.ToEmail) }
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
		}
	}
}