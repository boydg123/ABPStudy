using Abp.Application.Features;
using Abp.Collections.Extensions;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Abp.Authorization
{
    /// <summary>
    /// 权限定义上下文基类
    /// </summary>
    internal abstract class PermissionDefinitionContextBase : IPermissionDefinitionContext
    {
        /// <summary>
        /// 用于存储和操作权限的字典
        /// </summary>
        protected readonly PermissionDictionary Permissions;

        /// <summary>
        /// 构造函数
        /// </summary>
        protected PermissionDefinitionContextBase()
        {
            Permissions = new PermissionDictionary();
        }

        /// <summary>
        /// 创建权限
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="isGrantedByDefault">默认是否授权</param>
        /// <param name="description">简单描述</param>
        /// <param name="multiTenancySides">租户那一方使用</param>
        /// <returns></returns>
        public Permission CreatePermission(
            string name, 
            ILocalizableString displayName = null, 
            ILocalizableString description = null, 
            MultiTenancySides multiTenancySides = MultiTenancySides.Host | MultiTenancySides.Tenant,
            IFeatureDependency featureDependency = null)
        {
            if (Permissions.ContainsKey(name))
            {
                throw new AbpException("There is already a permission with name: " + name);
            }

            var permission = new Permission(name, displayName, description, multiTenancySides, featureDependency);
            Permissions[permission.Name] = permission;
            return permission;
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="name">权限名称</param>
        /// <returns></returns>
        public Permission GetPermissionOrNull(string name)
        {
            return Permissions.GetOrDefault(name);
        }
    }
}
