using System.Net.Mail;

namespace Abp.Net.Mail.Smtp
{
    /// <summary>
    /// Defines configurations to used by <see cref="SmtpClient"/> object.
    /// 定义使用<see cref="SmtpClient"/>的配置
    /// </summary>
    public interface ISmtpEmailSenderConfiguration : IEmailSenderConfiguration
    {
        /// <summary>
        /// SMTP Host name/IP.
        /// SMTP 主机名称/IP.
        /// </summary>
        string Host { get; }

        /// <summary>
        /// SMTP Port.
        /// SMTP 端口.
        /// </summary>
        int Port { get; }

        /// <summary>
        /// User name to login to SMTP server.
        /// 登录SMTP服务的用户名
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Password to login to SMTP server.
        /// 登录SMTP服务的密码
        /// </summary>
        string Password { get; }

        /// <summary>
        /// Domain name to login to SMTP server.
        /// 登录SMTP的域名
        /// </summary>
        string Domain { get; }

        /// <summary>
        /// Is SSL enabled?
        /// 是否使用SSL
        /// </summary>
        bool EnableSsl { get; }

        /// <summary>
        /// Use default credentials?
        /// 是否使用默认凭证
        /// </summary>
        bool UseDefaultCredentials { get; }
    }
}