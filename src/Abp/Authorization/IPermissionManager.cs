using System.Collections.Generic;
using Abp.MultiTenancy;

namespace Abp.Authorization
{
    /// <summary>
    /// Permission manager.
    /// 权限管理器
    /// </summary>
    public interface IPermissionManager
    {
        /// <summary>
        /// Gets <see cref="Permission"/> object with given <paramref name="name"/> or throws exception
        /// 通过给定的<paramref name="name"/>获取 <see cref="Permission"/> 对象 
        /// if there is no permission with given <paramref name="name"/>.
        /// 如果没有给定<paramref name="name"/>的权限，将抛出异常
        /// </summary>
        /// <param name="name">Unique name of the permission / 权限名称</param>
        Permission GetPermission(string name);

        /// <summary>
        /// Gets <see cref="Permission"/> object with given <paramref name="name"/> or returns null
        /// 通过给定的<paramref name="name"/>获取 <see cref="Permission"/> 对象 
        /// if there is no permission with given <paramref name="name"/>.
        /// 如果没有给定<paramref name="name"/>的权限，将返回null
        /// </summary>
        /// <param name="name">Unique name of the permission / 权限名称</param>
        Permission GetPermissionOrNull(string name);

        /// <summary>
        /// Gets all permissions.
        /// 获取所有权限
        /// </summary>
        /// <param name="tenancyFilter">Can be passed false to disable tenancy filter. / 可以传入false禁用租户过滤</param>
        IReadOnlyList<Permission> GetAllPermissions(bool tenancyFilter = true);

        /// <summary>
        /// Gets all permissions.
        /// 获取所有权限
        /// </summary>
        /// <param name="multiTenancySides">Multi-tenancy side to filter / 多租户过滤</param>
        IReadOnlyList<Permission> GetAllPermissions(MultiTenancySides multiTenancySides);
    }
}
