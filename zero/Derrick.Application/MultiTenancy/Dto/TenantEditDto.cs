using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Derrick.MultiTenancy.Dto
{
    /// <summary>
    /// 商户编辑Dto
    /// </summary>
    [AutoMap(typeof (Tenant))]
    public class TenantEditDto : EntityDto
    {
        /// <summary>
        /// 商户名称
        /// </summary>
        [Required]
        [StringLength(Tenant.MaxTenancyNameLength)]
        public string TenancyName { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        [StringLength(Tenant.MaxNameLength)]
        public string Name { get; set; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
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