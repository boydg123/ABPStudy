using System.Net.Mail;
using System.Threading.Tasks;

namespace Abp.Net.Mail
{
    /// <summary>
    /// This service can be used simply sending emails.
    /// 用于简单发送电子邮件
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Sends an email.
        /// 发送一封电子邮件
        /// </summary>
        Task SendAsync(string to, string subject, string body, bool isBodyHtml = true);

        /// <summary>
        /// Sends an email.
        /// 发送一封电子邮件
        /// </summary>
        void Send(string to, string subject, string body, bool isBodyHtml = true);

        /// <summary>
        /// Sends an email.
        /// 发送一封电子邮件
        /// </summary>
        Task SendAsync(string from, string to, string subject, string body, bool isBodyHtml = true);

        /// <summary>
        /// Sends an email.
        /// 发送一封电子邮件
        /// </summary>
        void Send(string from, string to, string subject, string body, bool isBodyHtml = true);

        /// <summary>
        /// Sends an email.
        /// 发送一封电子邮件
        /// </summary>
        /// <param name="mail">Mail to be sent / 将要发送的电子邮件</param>
        /// <param name="normalize">
        /// Should normalize email?If true, it sets sender address/name if it's not set before and makes mail encoding UTF-8. 
        /// 是否需要规范化电子邮件?如果需要，它将设置地址/名称（如果发送前没有设置）和设置电子邮件编码为UTF-8。
        /// </param>
        void Send(MailMessage mail, bool normalize = true);

        /// <summary>
        /// Sends an email.发送一封电子邮件
        /// </summary>
        /// <param name="mail">Mail to be sent / 将要发送的电子邮件</param>
        /// <param name="normalize">
        /// Should normalize email?If true, it sets sender address/name if it's not set before and makes mail encoding UTF-8.
        ///  是否需要规范化电子邮件?如果需要，它将设置地址/名称（如果发送前没有设置）和设置电子邮件编码为UTF-8。
        /// </param>
        Task SendAsync(MailMessage mail, bool normalize = true);
    }
}
