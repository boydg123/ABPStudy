using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Organizations;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Represents membership of a User to an OU.
    /// 标识一个用户成员的组织架构
    /// </summary>
    [Table("AbpUserOrganizationUnits")]
    public class UserOrganizationUnit : CreationAuditedEntity<long>, IMayHaveTenant
    {
        /// <summary>
        /// TenantId of this entity.
        /// 商户ID
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// Id of the User.
        /// 用户ID
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// Id of the <see cref="OrganizationUnit"/>.
        /// 组织ID
        /// </summary>
        public virtual long OrganizationUnitId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserOrganizationUnit"/> class.
        /// 构造函数
        /// </summary>
        public UserOrganizationUnit()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserOrganizationUnit"/> class.
        /// 构造函数
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="organizationUnitId">组织ID.</param>
        public UserOrganizationUnit(int? tenantId, long userId, long organizationUnitId)
        {
            TenantId = tenantId;
            UserId = userId;
            OrganizationUnitId = organizationUnitId;
        }
    }
}
