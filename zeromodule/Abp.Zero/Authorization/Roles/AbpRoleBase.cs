using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization.Roles
{
    /// <summary>
    /// 角色基类.
    /// </summary>
    [Table("AbpRoles")]
    public abstract class AbpRoleBase : FullAuditedEntity<int>, IRole<int>, IMayHaveTenant
    {
        /// <summary>
        /// <see cref="Name"/>属性的最大长度
        /// </summary>
        public const int MaxNameLength = 32;

        /// <summary>
        /// Tenant's Id, if this role is a tenant-level role. Null, if not.
        /// 商户ID，如果这个角色是商户级别的角色，NULL，否则不是
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// 角色的唯一名称
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }
    }
}