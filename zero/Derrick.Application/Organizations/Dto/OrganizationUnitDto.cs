using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Organizations;

namespace Derrick.Organizations.Dto
{
    /// <summary>
    /// 组织架构Dto
    /// </summary>
    [AutoMapFrom(typeof(OrganizationUnit))]
    public class OrganizationUnitDto : AuditedEntityDto<long>
    {
        /// <summary>
        /// 父ID
        /// </summary>
        public long? ParentId { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 显示名
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 成员数量
        /// </summary>
        public int MemberCount { get; set; }
    }
}