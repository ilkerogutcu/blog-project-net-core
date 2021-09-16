using Blog.Business.Abstract;
using Blog.Core.Entities.Mail;
using Blog.Core.Utilities.IoC;
using Blog.Core.Utilities.Mail;
using Blog.Entities.Concrete;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Aspects.Autofac.Exception;
using Blog.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Blog.Core.Utilities.MessageBrokers.RabbitMq;

namespace Blog.Business.Concrete
{
    public class AuthenticationMailManager : IAuthenticationMailService
    {
        private readonly string _baseUrl;
        private readonly IRabbitMqProducer _producer;

        public AuthenticationMailManager(IRabbitMqProducer producer)
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
            _baseUrl = configuration?.GetSection("BaseUrl").Value;
            _producer = producer;
        }

        #region Send verification email

        /// <summary>
        ///     Send verification email
        /// </summary>
        /// <param name="user"></param>
        /// <param name="verificationToken"></param>
        /// <returns>Verification url</returns>
        [ExceptionLogAspect(typeof(FileLogger))]
        public async Task<string> SendVerificationEmail(User user, string verificationToken)
        {
          
            // Generate token for confirm email
            verificationToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(verificationToken));

            // Generate endpoint url for verification url
            var endPointUrl = new Uri(string.Concat($"{_baseUrl}", "api/account/confirm-email/"));
            var verificationUrl = QueryHelpers.AddQueryString(endPointUrl.ToString(), "userId", user.Id);
            verificationUrl = QueryHelpers.AddQueryString(verificationUrl, "verificationToken", verificationToken);
            // Edit forgot password email template for reset password link
            var emailTemplatePath = Path.Combine(Environment.CurrentDirectory,
                @"MailTemplates\SendVerificationEmailTemplate.html");
            using var reader = new StreamReader(emailTemplatePath);
            var mailTemplate = await reader.ReadToEndAsync();
            reader.Close();

            await _producer.Publish(new ProducerModel
            {
                Model = new MailRequest
                {
                    ToEmail = user.Email,
                    Subject = "Please verification your email",
                    Body = mailTemplate.Replace("[verificationUrl]", verificationUrl)
                },
                QueueName = "send-email-queue"
            });

            return verificationUrl;
        }

        #endregion

        #region Send forgot password email

        /// <summary>
        ///     Send forgot password email
        /// </summary>
        /// <param name="user"></param>
        /// <param name="resetToken"></param>
        /// <returns></returns>
        public async Task SendForgotPasswordEmail(User user, string resetToken)
        {
            // Generate token for reset password
            resetToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));

            // Generate endpoint url for reset password
            var endPointUrl =
                new Uri(string.Concat($"{_baseUrl}", "api/account/reset-password/"));
            var resetTokenUrl = QueryHelpers.AddQueryString(endPointUrl.ToString(), "username", user.UserName);
            resetTokenUrl = QueryHelpers.AddQueryString(resetTokenUrl, "token", resetToken);

            // Edit forgot password email template for reset password link
            var emailTemplatePath = Path.Combine(Environment.CurrentDirectory,
                @"MailTemplates\SendForgotPasswordEmailTemplate.html");
            using var reader = new StreamReader(emailTemplatePath);
            var mailTemplate = await reader.ReadToEndAsync();
            reader.Close();
            
            await _producer.Publish(new ProducerModel
            {
                Model = new MailRequest
                {
                    ToEmail = user.Email,
                    Subject = "You have requested to reset your password",
                    Body = mailTemplate.Replace("[resetPasswordLink]", resetTokenUrl)
                },
                QueueName = "send-email-queue"
            });
        }

        #endregion

        #region Send 2fa token email

        public async Task SendTwoFactorCodeEmail(User user, string code)
        {
            // Edit forgot password email template for reset password link
            var emailTemplatePath = Path.Combine(Environment.CurrentDirectory,
                @"MailTemplates\Send2FAEmailTemplate.html");
            using var reader = new StreamReader(emailTemplatePath);
            var mailTemplate = await reader.ReadToEndAsync();
            reader.Close();
            await _producer.Publish(new ProducerModel
            {
                Model = new MailRequest
                {
                    ToEmail = user.Email,
                    Subject = $"Your code is {code}",
                    Body = mailTemplate.Replace("[2FACode]", code)
                },
                QueueName = "send-email-queue"
            });
        }
        #endregion
    }
}