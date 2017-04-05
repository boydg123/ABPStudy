using Abp.Configuration;
using Abp.Dependency;

namespace Abp.Net.Mail.Smtp
{
    /// <summary>
    /// Implementation of <see cref="ISmtpEmailSenderConfiguration"/> that reads settings from <see cref="ISettingManager"/>.
    /// 实现接口<see cref="ISmtpEmailSenderConfiguration"/>,它从<see cref="ISettingManager"/>读取设置。
    /// </summary>
    public class SmtpEmailSenderConfiguration : EmailSenderConfiguration, ISmtpEmailSenderConfiguration, ITransientDependency
    {
        /// <summary>
        /// SMTP Host name/IP.
        /// SMTP 主机名称/IP.
        /// </summary>
        public string Host
        {
            get { return GetNotEmptySettingValue(EmailSettingNames.Smtp.Host); }
        }

        /// <summary>
        /// SMTP Port.
        /// SMTP 端口.
        /// </summary>
        public int Port
        {
            get { return SettingManager.GetSettingValue<int>(EmailSettingNames.Smtp.Port); }
        }

        /// <summary>
        /// User name to login to SMTP server.
        /// 登录SMTP服务的用户名.
        /// </summary>
        public string UserName
        {
            get { return GetNotEmptySettingValue(EmailSettingNames.Smtp.UserName); }
        }

        /// <summary>
        /// Password to login to SMTP server.
        /// 登录SMTP服务的密码.
        /// </summary>
        public string Password
        {
            get { return GetNotEmptySettingValue(EmailSettingNames.Smtp.Password); }
        }

        /// <summary>
        /// Domain name to login to SMTP server.
        /// 登录SMTP的域名
        /// </summary>
        public string Domain
        {
            get { return SettingManager.GetSettingValue(EmailSettingNames.Smtp.Domain); }
        }

        /// <summary>
        /// Is SSL enabled?
        /// 是否使用SSL
        /// </summary>
        public bool EnableSsl
        {
            get { return SettingManager.GetSettingValue<bool>(EmailSettingNames.Smtp.EnableSsl); }
        }

        /// <summary>
        /// Use default credentials?
        /// 是否使用默认凭证
        /// </summary>
        public bool UseDefaultCredentials
        {
            get { return SettingManager.GetSettingValue<bool>(EmailSettingNames.Smtp.UseDefaultCredentials); }
        }

        /// <summary>
        /// Creates a new <see cref="SmtpEmailSenderConfiguration"/>.
        /// 构造函数
        /// </summary>
        /// <param name="settingManager">Setting manager / 设置管理器</param>
        public SmtpEmailSenderConfiguration(ISettingManager settingManager)
            : base(settingManager)
        {

        }
    }
}