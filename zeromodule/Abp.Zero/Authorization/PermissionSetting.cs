using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Abp.Authorization
{
    /// <summary>
    /// Used to grant/deny a permission for a role or user.
    /// 用于授予/拒绝角色或用户的权限
    /// </summary>
    [Table("AbpPermissions")]
    public abstract class PermissionSetting : CreationAuditedEntity<long>, IMayHaveTenant
    {
        /// <summary>
        /// Maximum length of the <see cref="Name"/> field.
        /// <see cref="Name"/>字段的最大长度
        /// </summary>
        public const int MaxNameLength = 128;

        /// <summary>
        /// 商户ID
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// Unique name of the permission.
        /// 权限名称
        /// </summary>
        [Required]
        [MaxLength(MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Is this role granted for this permission.Default value: true.
        /// 此角色是否被授予此权限，默认值：True
        /// </summary>
        public virtual bool IsGranted { get; set; }

        /// <summary>
        /// Creates a new <see cref="PermissionSetting"/> entity.
        /// 构造函数
        /// </summary>
        protected PermissionSetting()
        {
            IsGranted = true;
        }
    }
}
