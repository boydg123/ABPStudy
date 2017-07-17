using System.ComponentModel.DataAnnotations;
using Abp.Organizations;

namespace Derrick.Organizations.Dto
{
    /// <summary>
    /// 创建组织架构Input
    /// </summary>
    public class CreateOrganizationUnitInput
    {
        /// <summary>
        /// 父ID
        /// </summary>
        public long? ParentId { get; set; }
        /// <summary>
        /// 显示名
        /// </summary>
        [Required]
        [StringLength(OrganizationUnit.MaxDisplayNameLength)]
        public string DisplayName { get; set; } 
    }
}