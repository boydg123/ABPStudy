using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.MultiTenancy;
using Derrick.Authorization.Users;

namespace Derrick.MultiTenancy.Dto
{
    /// <summary>
    /// 创建商户Input
    /// </summary>
    public class CreateTenantInput
    {
        /// <summary>
        /// 商户名称
        /// </summary>
        [Required]
        [StringLength(AbpTenantBase.MaxTenancyNameLength)]
        [RegularExpression(Tenant.TenancyNameRegex)]
        public string TenancyName { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        [StringLength(Tenant.MaxNameLength)]
        public string Name { get; set; }
        /// <summary>
        /// 管理员邮箱
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(User.MaxEmailAddressLength)]
        public string AdminEmailAddress { get; set; }
        /// <summary>
        /// 管理员密码
        /// </summary>
        [StringLength(User.MaxPasswordLength)]
        public string AdminPassword { get; set; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        [MaxLength(AbpTenantBase.MaxConnectionStringLength)]
        public string ConnectionString { get; set; }
        /// <summary>
        /// 是否在下次登陆时修改密码
        /// </summary>
        public bool ShouldChangePasswordOnNextLogin { get; set; }
        /// <summary>
        /// 发送激活邮箱
        /// </summary>
        public bool SendActivationEmail { get; set; }
        /// <summary>
        /// 版本ID
        /// </summary>
        public int? EditionId { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }
    }
}