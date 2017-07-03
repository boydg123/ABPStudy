using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Authorization.Users;
using Abp.Domain.Entities.Auditing;

namespace Abp.Authorization.Roles
{
    /// <summary>
    /// Represents a role in an application. A role is used to group permissions.
    /// 表示应用程序中的角色，角色用于分组权限
    /// </summary>
    /// <remarks> 
    /// Application should use permissions to check if user is granted to perform an operation.Checking 'if a user has a role' is not possible until the role is static (<see cref="IsStatic"/>).
    /// 应用程序应该使用权限检查是否允许用户执行操作。在角色是静态的之前，检查用户是否有角色是不可能的
    /// Static roles can be used in the code and can not be deleted by users.Non-static (dynamic) roles can be added/removed by users and we can not know their name while coding
    /// 静态角色能在代码中使用并且不能被用户删除.非静态(动态)角色能被用户添加/删除，在编码时我们不能知道他们的名称。
    /// A user can have multiple roles. Thus, user will have all permissions of all assigned roles.
    /// 一个用户可以有多个角色，因此，用户将有所有角色的权限。
    /// </remarks>
    public abstract class AbpRole<TUser> : AbpRoleBase, IFullAudited<TUser>
        where TUser : AbpUser<TUser>
    {
        /// <summary>
        /// <see cref="DisplayName"/>属性的最大长度
        /// </summary>
        public const int MaxDisplayNameLength = 64;

        /// <summary>
        /// 角色的显示名称
        /// </summary>
        [Required]
        [StringLength(MaxDisplayNameLength)]
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Is this a static role?Static roles can not be deleted, can not change their name.They can be used programmatically.
        /// 是否是静态角色，静态角色不能被删除，不能修改名称。能在程序中使用
        /// </summary>
        public virtual bool IsStatic { get; set; }

        /// <summary>
        /// Is this role will be assigned to new users as default?
        /// 此角色是否默认分配给新用户
        /// </summary>
        public virtual bool IsDefault { get; set; }

        /// <summary>
        /// List of permissions of the role.
        /// 角色的权限列表
        /// </summary>
        [ForeignKey("RoleId")]
        public virtual ICollection<RolePermissionSetting> Permissions { get; set; }
        /// <summary>
        /// 删除者
        /// </summary>
        public virtual TUser DeleterUser { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public virtual TUser CreatorUser { get; set; }
        /// <summary>
        /// 最后修改人
        /// </summary>
        public virtual TUser LastModifierUser { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public AbpRole()
        {
            Name = Guid.NewGuid().ToString("N");
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tenantId">TenantId or null (if this is not a tenant-level role) / 商户ID或NULL(如果这不是一个商户级别的角色)</param>
        /// <param name="displayName">Display name of the role / 角色的显示名称</param>
        public AbpRole(int? tenantId, string displayName)
            : this()
        {
            TenantId = tenantId;
            DisplayName = displayName;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tenantId">TenantId or null (if this is not a tenant-level role) / 商户ID或NULL(如果这不是一个商户级别的角色)</param>
        /// <param name="name">角色的唯一名称</param>
        /// <param name="displayName">角色的显示名</param>
        public AbpRole(int? tenantId, string name, string displayName)
            : this(tenantId, displayName)
        {
            Name = name;
        }
        /// <summary>
        /// 字符串重写
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[Role {0}, Name={1}]", Id, Name);
        }
    }
}
