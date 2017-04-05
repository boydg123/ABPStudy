using Abp.Application.Features;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Abp.Authorization
{
    /// <summary>
    /// This context is used on <see cref="AuthorizationProvider.SetPermissions"/> method.
    /// 该上下文用于方法<see cref="AuthorizationProvider.SetPermissions"/> .
    /// </summary>
    public interface IPermissionDefinitionContext
    {
        /// <summary>
        /// Creates a new permission under this group.
        /// 创建一个Permission对象
        /// </summary>
        /// <param name="name">Unique name of the permission / 权限唯一名称</param>
        /// <param name="displayName">Display name of the permission / 显示名称</param>
        /// <param name="description">A brief description for this permission / 简要的描述</param>
        /// <param name="multiTenancySides">Which side can use this permission / 哪一边使用该权限</param>
        /// <param name="featureDependency">Depended feature(s) of this permission / 此权限的依赖功能</param>
        /// <returns>New created permission / 新创建的权限</returns>
        Permission CreatePermission(
            string name, 
            ILocalizableString displayName = null, 
            ILocalizableString description = null, 
            MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant,
            IFeatureDependency featureDependency = null
            );

        /// <summary>
        /// Gets a permission with given name or null if can not find.
        /// 获取一个给定名称的权限，如果不存在返回null
        /// </summary>
        /// <param name="name">Unique name of the permission / 唯一的权限名称</param>
        /// <returns>Permission object or null / Permission对象或者 null</returns>
        Permission GetPermissionOrNull(string name);
    }
}