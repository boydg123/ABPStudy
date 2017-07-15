namespace Derrick.Configuration
{
    /// <summary>
    /// 定义应用程序内的设置名称常量。通过<see cref="AppSettingProvider"/>查看设置定义
    /// </summary>
    public static class AppSettings
    {
        /// <summary>
        /// 常规设置
        /// </summary>
        public static class General
        {
            public const string WebSiteRootAddress = "App.General.WebSiteRootAddress";
        }

        /// <summary>
        /// 商户设置
        /// </summary>
        public static class TenantManagement
        {
            public const string AllowSelfRegistration = "App.TenantManagement.AllowSelfRegistration";
            public const string IsNewRegisteredTenantActiveByDefault = "App.TenantManagement.IsNewRegisteredTenantActiveByDefault";
            public const string UseCaptchaOnRegistration = "App.TenantManagement.UseCaptchaOnRegistration";
            public const string DefaultEdition = "App.TenantManagement.DefaultEdition";
        }

        /// <summary>
        /// 用户设置
        /// </summary>
        public static class UserManagement
        {
            public const string AllowSelfRegistration = "App.UserManagement.AllowSelfRegistration";
            public const string IsNewRegisteredUserActiveByDefault = "App.UserManagement.IsNewRegisteredUserActiveByDefault";
            public const string UseCaptchaOnRegistration = "App.UserManagement.UseCaptchaOnRegistration";
        }

        /// <summary>
        /// 安全设置
        /// </summary>
        public static class Security
        {
            public const string PasswordComplexity = "App.Security.PasswordComplexity";
        }
    }
}