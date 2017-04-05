using System.Net.Mail;
using System.Threading.Tasks;
using Castle.Core.Logging;

namespace Abp.Net.Mail
{
    /// <summary>
    /// This class is an implementation of <see cref="IEmailSender"/> as similar to null pattern.It does not send emails but logs them.
    /// 此类是接口 <see cref="IEmailSender"/>的null模式实现，它不发送邮件，但会记录邮件。
    /// </summary>
    public class NullEmailSender : EmailSenderBase
    {
        public ILogger Logger { get; set; }

        /// <summary>
        /// Creates a new <see cref="NullEmailSender"/> object.
        /// 构造函数
        /// </summary>
        /// <param name="configuration">Configuration / 邮件配置</param>
        public NullEmailSender(IEmailSenderConfiguration configuration)
            : base(configuration)
        {
            Logger = NullLogger.Instance;
        }

        /// <summary>
        /// 发送一封电子邮件
        /// </summary>
        /// <param name="mail">邮件</param>
        /// <returns></returns>
        protected override Task SendEmailAsync(MailMessage mail)
        {
            Logger.Warn("USING NullEmailSender!");
            Logger.Debug("SendEmailAsync:");
            LogEmail(mail);
            return Task.FromResult(0);
        }

        /// <summary>
        /// 发送一封电子邮件
        /// </summary>
        /// <param name="mail">邮件</param>
        protected override void SendEmail(MailMessage mail)
        {
            Logger.Warn("USING NullEmailSender!");
            Logger.Debug("SendEmail:");
            LogEmail(mail);
        }

        /// <summary>
        /// 发送一封电子邮件
        /// </summary>
        /// <param name="mail">邮件</param>
        private void LogEmail(MailMessage mail)
        {
            Logger.Debug(mail.To.ToString());
            Logger.Debug(mail.CC.ToString());
            Logger.Debug(mail.Subject);
            Logger.Debug(mail.Body);
        }
    }
}