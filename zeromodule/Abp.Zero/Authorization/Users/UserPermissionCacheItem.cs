using System;
using System.Collections.Generic;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Used to cache roles and permissions of a user.
    /// 用于缓存用户的角色和权限
    /// </summary>
    [Serializable]
    public class UserPermissionCacheItem
    {
        /// <summary>
        /// 缓存存储名
        /// </summary>
        public const string CacheStoreName = "AbpZeroUserPermissions";

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 角色ID集合
        /// </summary>
        public List<int> RoleIds { get; set; }

        /// <summary>
        /// 授予权限的集合
        /// </summary>
        public HashSet<string> GrantedPermissions { get; set; }

        /// <summary>
        /// 禁止权限的集合
        /// </summary>
        public HashSet<string> ProhibitedPermissions { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserPermissionCacheItem()
        {
            RoleIds = new List<int>();
            GrantedPermissions = new HashSet<string>();
            ProhibitedPermissions = new HashSet<string>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userId">用户ID</param>
        public UserPermissionCacheItem(long userId)
            : this()
        {
            UserId = userId;
        }
    }
}
