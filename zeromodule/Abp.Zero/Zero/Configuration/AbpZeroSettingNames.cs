namespace Abp.Zero.Configuration
{
    /// <summary>
    /// ABP Zero设置名称
    /// </summary>
    public static class AbpZeroSettingNames
    {
        /// <summary>
        /// 用户管理
        /// </summary>
        public static class UserManagement
        {
            /// <summary>
            /// 登录时候电子邮件是否需要确认
            /// </summary>
            public const string IsEmailConfirmationRequiredForLogin = "Abp.Zero.UserManagement.IsEmailConfirmationRequiredForLogin";

            /// <summary>
            /// 用户锁定
            /// </summary>
            public static class UserLockOut
            {
                /// <summary>
                /// 用户锁定是否启用
                /// </summary>
                public const string IsEnabled = "Abp.Zero.UserManagement.UserLockOut.IsEnabled";

                /// <summary>
                /// 锁定前最大失败访问尝试
                /// </summary>
                public const string MaxFailedAccessAttemptsBeforeLockout = "Abp.Zero.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout";

                /// <summary>
                /// 账户默认锁定秒
                /// </summary>
                public const string DefaultAccountLockoutSeconds = "Abp.Zero.UserManagement.UserLockOut.DefaultAccountLockoutSeconds";
            }
            /// <summary>
            /// 双因子登录
            /// </summary>
            public static class TwoFactorLogin
            {
                /// <summary>
                /// 是否启用双因子登录
                /// </summary>
                public const string IsEnabled = "Abp.Zero.UserManagement.TwoFactorLogin.IsEnabled";

                /// <summary>
                /// 是否启用电子邮件提供程序
                /// </summary>
                public const string IsEmailProviderEnabled = "Abp.Zero.UserManagement.TwoFactorLogin.IsEmailProviderEnabled";

                /// <summary>
                /// 双因子登录是否提供短信服务
                /// </summary>
                public const string IsSmsProviderEnabled = "Abp.Zero.UserManagement.TwoFactorLogin.IsSmsProviderEnabled";

                /// <summary>
                /// 浏览器是否记住
                /// </summary>
                public const string IsRememberBrowserEnabled = "Abp.Zero.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled";
            }
        }
        /// <summary>
        /// 组织架构
        /// </summary>
        public static class OrganizationUnits
        {
            /// <summary>
            /// 组织的最大成员数量
            /// </summary>
            public const string MaxUserMembershipCount = "Abp.Zero.OrganizationUnits.MaxUserMembershipCount";
        }
    }
}