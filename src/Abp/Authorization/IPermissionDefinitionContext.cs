using Abp.Application.Features;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Abp.Authorization
{
    /// <summary>
    /// 该上下文用于方法<see cref="AuthorizationProvider.SetPermissions"/> .
    /// </summary>
    public interface IPermissionDefinitionContext
    {
        /// <summary>
        /// 创建一个Permission对象
        /// </summary>
        /// <param name="name">权限唯一名称</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="description">简要的描述</param>
        /// <param name="multiTenancySides">哪一边使用该权限</param>
        /// <param name="featureDependency">此权限的依赖功能</param>
        /// <returns>新创建的权限</returns>
        Permission CreatePermission(
            string name, 
            ILocalizableString displayName = null, 
            ILocalizableString description = null, 
            MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant,
            IFeatureDependency featureDependency = null
            );

        /// <summary>
        /// 获取一个给定名称的权限，如果不存在返回null
        /// </summary>
        /// <param name="name">唯一的权限名称</param>
        /// <returns>Permission对象或者 null</returns>
        Permission GetPermissionOrNull(string name);
    }
}