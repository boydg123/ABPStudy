namespace Derrick.Configuration.Tenants.Dto
{
    /// <summary>
    /// 商户用户管理设置编辑Dto
    /// </summary>
    public class TenantUserManagementSettingsEditDto
    {
        /// <summary>
        /// 是否允许自注册
        /// </summary>
        public bool AllowSelfRegistration { get; set; }
        /// <summary>
        /// 是否默认激活新注册用户
        /// </summary>
        public bool IsNewRegisteredUserActiveByDefault { get; set; }
        /// <summary>
        /// 登录时是否需要邮箱确认
        /// </summary>
        public bool IsEmailConfirmationRequiredForLogin { get; set; }
        /// <summary>
        /// 是否使用验证码注册
        /// </summary>
        public bool UseCaptchaOnRegistration { get; set; }
    }
}