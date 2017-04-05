namespace Abp.Net.Mail
{
    /// <summary>
    /// Declares names of the settings defined by <see cref="EmailSettingProvider"/>.
    /// 申明 <see cref="EmailSettingProvider"/>定义的设置名称
    /// </summary>
    public static class EmailSettingNames
    {
        /// <summary>
        /// Abp.Net.Mail.DefaultFromAddress
        /// 默认From地址
        /// </summary>
        public const string DefaultFromAddress = "Abp.Net.Mail.DefaultFromAddress";

        /// <summary>
        /// Abp.Net.Mail.DefaultFromDisplayName
        /// 默认From显示名称
        /// </summary>
        public const string DefaultFromDisplayName = "Abp.Net.Mail.DefaultFromDisplayName";

        /// <summary>
        /// SMTP related email settings.
        /// 与邮箱设置相关联的SMTP
        /// </summary>
        public static class Smtp
        {
            /// <summary>
            /// Abp.Net.Mail.Smtp.Host
            /// 主机
            /// </summary>
            public const string Host = "Abp.Net.Mail.Smtp.Host";

            /// <summary>
            /// Abp.Net.Mail.Smtp.Port
            /// 端口
            /// </summary>
            public const string Port = "Abp.Net.Mail.Smtp.Port";

            /// <summary>
            /// Abp.Net.Mail.Smtp.UserName
            /// 用户名
            /// </summary>
            public const string UserName = "Abp.Net.Mail.Smtp.UserName";

            /// <summary>
            /// Abp.Net.Mail.Smtp.Password
            /// 密码
            /// </summary>
            public const string Password = "Abp.Net.Mail.Smtp.Password";

            /// <summary>
            /// Abp.Net.Mail.Smtp.Domain
            /// 域
            /// </summary>
            public const string Domain = "Abp.Net.Mail.Smtp.Domain";

            /// <summary>
            /// Abp.Net.Mail.Smtp.EnableSsl
            /// 是否使用SSL
            /// </summary>
            public const string EnableSsl = "Abp.Net.Mail.Smtp.EnableSsl";

            /// <summary>
            /// Abp.Net.Mail.Smtp.UseDefaultCredentials
            /// 使用默认证书
            /// </summary>
            public const string UseDefaultCredentials = "Abp.Net.Mail.Smtp.UseDefaultCredentials";
        }
    }
}