using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Extensions;

namespace Abp.Net.Mail.Smtp
{
    /// <summary>
    /// Used to send emails over SMTP.
    /// 使用SMTP发送电子邮件
    /// </summary>
    public class SmtpEmailSender : EmailSenderBase, ISmtpEmailSender, ITransientDependency
    {
        /// <summary>
        /// SMTP邮件发送配置
        /// </summary>
        private readonly ISmtpEmailSenderConfiguration _configuration;

        /// <summary>
        /// Creates a new <see cref="SmtpEmailSender"/>.
        /// 构造函数
        /// </summary>
        /// <param name="configuration">Configuration / SMTP邮件发送配置</param>
        public SmtpEmailSender(ISmtpEmailSenderConfiguration configuration)
            : base(configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 创建或者配置<see cref="SmtpClient"/> 对象，并发送电子邮件
        /// </summary>
        /// <returns>用来准备发送邮件的对象<see cref="SmtpClient"/></returns>
        public SmtpClient BuildClient()
        {
            var host = _configuration.Host;
            var port = _configuration.Port;

            var smtpClient = new SmtpClient(host, port);
            try
            {
                if (_configuration.EnableSsl)
                {
                    smtpClient.EnableSsl = true;
                }

                if (_configuration.UseDefaultCredentials)
                {
                    smtpClient.UseDefaultCredentials = true;
                }
                else
                {
                    smtpClient.UseDefaultCredentials = false;

                    var userName = _configuration.UserName;
                    if (!userName.IsNullOrEmpty())
                    {
                        var password = _configuration.Password;
                        var domain = _configuration.Domain;
                        smtpClient.Credentials = !domain.IsNullOrEmpty()
                            ? new NetworkCredential(userName, password, domain)
                            : new NetworkCredential(userName, password);
                    }
                }

                return smtpClient;
            }
            catch
            {
                smtpClient.Dispose();
                throw;
            }
        }

        /// <summary>
        /// 在继承类中需要实现此方法 
        /// </summary>
        /// <param name="mail">需要发送的电子邮件</param>
        protected override async Task SendEmailAsync(MailMessage mail)
        {
            using (var smtpClient = BuildClient())
            {
                await smtpClient.SendMailAsync(mail);
            }
        }

        /// <summary>
        /// 在继承类中需要实现此方法 
        /// </summary>
        /// <param name="mail">需要发送的电子邮件</param>
        protected override void SendEmail(MailMessage mail)
        {
            using (var smtpClient = BuildClient())
            {
                smtpClient.Send(mail);
            }
        }
    }
}