namespace Derrick.Configuration.Host.Dto
{
    /// <summary>
    /// 双因子登录设置编辑Dto
    /// </summary>
    public class TwoFactorLoginSettingsEditDto
    {
        /// <summary>
        /// 应用程序是否启用
        /// </summary>
        public bool IsEnabledForApplication { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// 是否启用邮件提供器
        /// </summary>
        public bool IsEmailProviderEnabled { get; set; }
        /// <summary>
        /// 是否启用短信提供器
        /// </summary>
        public bool IsSmsProviderEnabled { get; set; }
        /// <summary>
        /// 是否记住浏览器设置
        /// </summary>
        public bool IsRememberBrowserEnabled { get; set; }
    }
}