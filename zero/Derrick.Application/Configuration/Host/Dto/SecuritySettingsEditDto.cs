using Derrick.Security;

namespace Derrick.Configuration.Host.Dto
{
    /// <summary>
    /// 安全设置编辑Dto
    /// </summary>
    public class SecuritySettingsEditDto
    {
        /// <summary>
        /// 是否使用默认密码复杂度设置
        /// </summary>
        public bool UseDefaultPasswordComplexitySettings { get; set; }
        /// <summary>
        /// 密码复杂度设置
        /// </summary>
        public PasswordComplexitySetting PasswordComplexity { get; set; }
        /// <summary>
        /// 默认密码复杂度设置
        /// </summary>
        public PasswordComplexitySetting DefaultPasswordComplexity { get; set; }
        /// <summary>
        /// 用户锁定设置编辑Dto
        /// </summary>
        public UserLockOutSettingsEditDto UserLockOut { get; set; }
        /// <summary>
        /// 双因子登录设置编辑Dto
        /// </summary>
        public TwoFactorLoginSettingsEditDto TwoFactorLogin { get; set; }
    }
}