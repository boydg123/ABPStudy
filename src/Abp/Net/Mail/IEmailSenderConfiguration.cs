namespace Abp.Net.Mail
{
    /// <summary>
    /// Defines configurations used while sending emails.
    /// 定义发送电子邮件时使用的配置
    /// </summary>
    public interface IEmailSenderConfiguration
    {
        /// <summary>
        /// Default from address.
        /// 默认from地址
        /// </summary>
        string DefaultFromAddress { get; }

        /// <summary>
        /// Default display name.
        /// 默认显示名称
        /// </summary>
        string DefaultFromDisplayName { get; }
    }
}