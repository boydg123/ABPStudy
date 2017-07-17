using System.ComponentModel.DataAnnotations;

namespace Derrick.Configuration.Host.Dto
{
    /// <summary>
    /// 宿主设置编辑Dto
    /// </summary>
    public class HostSettingsEditDto
    {
        /// <summary>
        /// 常规设置DTO
        /// </summary>
        [Required]
        public GeneralSettingsEditDto General { get; set; }
        /// <summary>
        /// 宿主用户管理设置编辑Dto
        /// </summary>
        [Required]
        public HostUserManagementSettingsEditDto UserManagement { get; set; }
        /// <summary>
        /// 邮件设置编辑Dto
        /// </summary>
        [Required]
        public EmailSettingsEditDto Email { get; set; }
        /// <summary>
        /// 商户管理设置编辑Dto
        /// </summary>
        [Required]
        public TenantManagementSettingsEditDto TenantManagement { get; set; }
        /// <summary>
        /// 安全设置编辑Dto
        /// </summary>
        [Required]
        public SecuritySettingsEditDto Security { get; set; }
        
    }
}