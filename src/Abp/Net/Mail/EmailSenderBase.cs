using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Abp.Extensions;

namespace Abp.Net.Mail
{
    /// <summary>
    /// This class can be used as base to implement <see cref="IEmailSender"/>.
    /// 可作为实现接口<see cref="IEmailSender"/>的基类
    /// </summary>
    public abstract class EmailSenderBase : IEmailSender
    {
        /// <summary>
        /// 邮件配置
        /// </summary>
        private readonly IEmailSenderConfiguration _configuration;

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        /// <param name="configuration">Configuration</param>
        protected EmailSenderBase(IEmailSenderConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 发送一封电子邮件
        /// </summary>
        public async Task SendAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendAsync(_configuration.DefaultFromAddress, to, subject, body, isBodyHtml);
        }

        /// <summary>
        /// 发送一封电子邮件
        /// </summary>
        public void Send(string to, string subject, string body, bool isBodyHtml = true)
        {
            Send(_configuration.DefaultFromAddress, to, subject, body, isBodyHtml);
        }

        /// <summary>
        /// 发送一封电子邮件
        /// </summary>
        public async Task SendAsync(string from, string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendAsync(new MailMessage(from, to, subject, body) { IsBodyHtml = isBodyHtml });
        }

        /// <summary>
        /// 发送一封电子邮件
        /// </summary>
        public void Send(string from, string to, string subject, string body, bool isBodyHtml = true)
        {
            Send(new MailMessage(from, to, subject, body) { IsBodyHtml = isBodyHtml });
        }

        /// <summary>
        /// 发送一封电子邮件
        /// </summary>
        /// <param name="mail">将要发送的电子邮件</param>
        /// <param name="normalize">
        /// 是否需要规范化电子邮件?如果需要，它将设置地址/名称（如果发送前没有设置）和设置电子邮件编码为UTF-8。
        /// </param>
        public async Task SendAsync(MailMessage mail, bool normalize = true)
        {
            if (normalize)
            {
                NormalizeMail(mail);
            }

            await SendEmailAsync(mail);
        }

        /// <summary>
        /// 发送一封电子邮件
        /// </summary>
        /// <param name="mail">将要发送的电子邮件</param>
        /// <param name="normalize">
        /// 是否需要规范化电子邮件?如果需要，它将设置地址/名称（如果发送前没有设置）和设置电子邮件编码为UTF-8。
        /// </param>
        public void Send(MailMessage mail, bool normalize = true)
        {
            if (normalize)
            {
                NormalizeMail(mail);
            }

            SendEmail(mail);
        }

        /// <summary>
        /// Should implement this method to send email in derived classes.
        /// 在继承类中需要实现此方法
        /// </summary>
        /// <param name="mail">Mail to be sent / 需要发送的电子邮件</param>
        protected abstract Task SendEmailAsync(MailMessage mail);

        /// <summary>
        /// Should implement this method to send email in derived classes.
        /// 在继承类中需要实现此方法
        /// </summary>
        /// <param name="mail">Mail to be sent / 需要发送的电子邮件</param>
        protected abstract void SendEmail(MailMessage mail);

        /// <summary>
        /// Normalizes given email.Fills <see cref="MailMessage.From"/> if it's not filled before.Sets encodings to UTF8 if they are not set before.
        /// 规范化给定的邮件,如果没有填写<see cref="MailMessage.From"/>，将会被上。如果没有设置编码，将会被设置成UTF8
        /// </summary>
        /// <param name="mail">Mail to be normalized / 将会被规范化的邮件</param>
        protected virtual void NormalizeMail(MailMessage mail)
        {
            if (mail.From == null || mail.From.Address.IsNullOrEmpty())
            {
                mail.From = new MailAddress(
                    _configuration.DefaultFromAddress,
                    _configuration.DefaultFromDisplayName,
                    Encoding.UTF8
                    );
            }

            if (mail.HeadersEncoding == null)
            {
                mail.HeadersEncoding = Encoding.UTF8;
            }

            if (mail.SubjectEncoding == null)
            {
                mail.SubjectEncoding = Encoding.UTF8;
            }

            if (mail.BodyEncoding == null)
            {
                mail.BodyEncoding = Encoding.UTF8;
            }
        }
    }
}