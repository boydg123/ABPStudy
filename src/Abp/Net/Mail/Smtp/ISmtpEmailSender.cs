using System.Net.Mail;

namespace Abp.Net.Mail.Smtp
{
    /// <summary>
    /// Used to send emails over SMTP.
    /// 通过SMTP发送电子邮件
    /// </summary>
    public interface ISmtpEmailSender : IEmailSender
    {
        /// <summary>
        /// Creates and configures new <see cref="SmtpClient"/> object to send emails.
        /// 创建或者配置<see cref="SmtpClient"/> 对象，并发送电子邮件
        /// </summary>
        /// <returns>
        /// An <see cref="SmtpClient"/> object that is ready to send emails.
        /// 用来准备发送邮件的对象<see cref="SmtpClient"/>
        /// </returns>
        SmtpClient BuildClient();
    }
}