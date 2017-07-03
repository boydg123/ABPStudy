using System;
using System.Collections.Generic;

namespace Abp.Authorization.Roles
{
    /// <summary>
    /// Used to cache permissions of a role.
    /// 用于缓存一个角色的权限
    /// </summary>
    [Serializable]
    public class RolePermissionCacheItem
    {
        /// <summary>
        /// 缓存存储名称
        /// </summary>
        public const string CacheStoreName = "AbpZeroRolePermissions";
        /// <summary>
        /// 角色ID
        /// </summary>
        public long RoleId { get; set; }
        /// <summary>
        /// 授予权限的集合
        /// </summary>
        public HashSet<string> GrantedPermissions { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public RolePermissionCacheItem()
        {
            GrantedPermissions = new HashSet<string>();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="roleId">角色ID</param>
        public RolePermissionCacheItem(int roleId)
            : this()
        {
            RoleId = roleId;
        }
    }
}