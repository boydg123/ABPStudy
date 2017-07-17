namespace Derrick.Configuration.Host.Dto
{
    /// <summary>
    /// 邮件设置编辑Dto
    /// </summary>
    public class EmailSettingsEditDto
    {
        //No validation is done, since we may don't want to use email system.
        /// <summary>
        /// 默认地址
        /// </summary>
        public string DefaultFromAddress { get; set; }
        /// <summary>
        /// 默认显示名
        /// </summary>
        public string DefaultFromDisplayName { get; set; }
        /// <summary>
        /// SMTP宿主
        /// </summary>
        public string SmtpHost { get; set; }
        /// <summary>
        /// SMTP端口
        /// </summary>
        public int SmtpPort { get; set; }
        /// <summary>
        /// SMTP用户名
        /// </summary>
        public string SmtpUserName { get; set; }
        /// <summary>
        /// SMTP密码
        /// </summary>
        public string SmtpPassword { get; set; }
        /// <summary>
        /// SMTP域名
        /// </summary>
        public string SmtpDomain { get; set; }
        /// <summary>
        /// 是否启用SMTP SSL
        /// </summary>
        public bool SmtpEnableSsl { get; set; }
        /// <summary>
        /// 是否使用SMTP默认的认证
        /// </summary>
        public bool SmtpUseDefaultCredentials { get; set; }
    }
}