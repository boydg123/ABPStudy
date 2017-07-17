namespace Derrick.Configuration.Host.Dto
{
    /// <summary>
    /// 商户管理设置编辑Dto
    /// </summary>
    public class TenantManagementSettingsEditDto
    {
        /// <summary>
        /// 是否允许自注册
        /// </summary>
        public bool AllowSelfRegistration { get; set; }
        /// <summary>
        /// 默认情况下是否激活新商户
        /// </summary>
        public bool IsNewRegisteredTenantActiveByDefault { get; set; }
        /// <summary>
        /// 是否使用验证码注册
        /// </summary>
        public bool UseCaptchaOnRegistration { get; set; }
        /// <summary>
        /// 默认版本ID
        /// </summary>
        public int? DefaultEditionId { get; set; }
        
    }
}