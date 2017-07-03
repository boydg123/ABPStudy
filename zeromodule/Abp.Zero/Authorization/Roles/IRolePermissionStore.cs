using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Authorization.Users;

namespace Abp.Authorization.Roles
{
    /// <summary>
    /// Used to perform permission database operations for a role.
    /// 用于为角色执行权限数据库操作
    /// </summary>
    public interface IRolePermissionStore<TRole, TUser>
        where TRole : AbpRole<TUser>
        where TUser : AbpUser<TUser>
    {
        /// <summary>
        /// Adds a permission grant setting to a role.
        /// 为角色添加一个全选授予设置
        /// </summary>
        /// <param name="role">角色对象</param>
        /// <param name="permissionGrant">Permission grant setting info / 权限授予信息</param>
        Task AddPermissionAsync(TRole role, PermissionGrantInfo permissionGrant);

        /// <summary>
        /// Removes a permission grant setting from a role.
        /// 为角色移除一个权限授予设置
        /// </summary>
        /// <param name="role">角色</param>
        /// <param name="permissionGrant">Permission grant setting info / 权限授予信息</param>
        Task RemovePermissionAsync(TRole role, PermissionGrantInfo permissionGrant);

        /// <summary>
        /// Gets permission grant setting informations for a role.
        /// 为角色获取权限授予设置信息集合
        /// </summary>
        /// <param name="role">角色</param>
        /// <returns>List of permission setting informations / 权限设置信息集合</returns>
        Task<IList<PermissionGrantInfo>> GetPermissionsAsync(TRole role);

        /// <summary>
        /// Gets permission grant setting informations for a role.
        /// 为角色获取权限授予设置信息集合
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>List of permission setting informations / 权限设置信息集合</returns>
        Task<IList<PermissionGrantInfo>> GetPermissionsAsync(int roleId);

        /// <summary>
        /// Checks whether a role has a permission grant setting info.
        /// 检查角色是否有权限授予设置信息
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="permissionGrant">Permission grant setting info / 权限授予设置信息</param>
        /// <returns></returns>
        Task<bool> HasPermissionAsync(int roleId, PermissionGrantInfo permissionGrant);

        /// <summary>
        /// Deleted all permission settings for a role.
        /// 为角色删除所有权限设置
        /// </summary>
        /// <param name="role">角色</param>
        Task RemoveAllPermissionSettingsAsync(TRole role);
    }
}