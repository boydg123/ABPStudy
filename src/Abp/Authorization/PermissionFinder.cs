using System.Collections.Generic;
using System.Collections.Immutable;

namespace Abp.Authorization
{
    /// <summary>
    /// This class is used to get permissions out of the system.Normally, you should inject and use <see cref="IPermissionManager"/> and use it. This class can be used in database migrations or in unit tests where Abp is not initialized.
    /// 此类用于从系统中获取一个权限，一般地，应该使用依赖注入和<see cref="IPermissionManager"/>来获取权限。此类用于在ABPT系统未初始化时进行单元测试，或者数据迁移
    /// </summary>
    public static class PermissionFinder
    {
        /// <summary>
        /// Collects and gets all permissions in given providers.This method can be used to get permissions in database migrations or in unit tests where Abp is not initialized.Otherwise, use <see cref="IPermissionManager.GetAllPermissions(bool)"/> method.
        /// 获取给定提供者（provider)中的所有权限,此方法用于在ABPT系统未初始化时进行单元测试，或者数据迁移,否则, 使用方法<see cref="IPermissionManager.GetAllPermissions(bool)"/>
        /// </summary>
        /// <param name="authorizationProviders">Authorization providers / 授权提供者</param>
        /// <returns>List of permissions / 权限列表</returns>
        /// <remarks>
        /// This method creates instances of <see cref="authorizationProviders"/> by order and calls <see cref="AuthorizationProvider.SetPermissions"/> to build permission list.So, providers should not use dependency injection.
        /// 此方法按顺序创建实例，调用<see cref="AuthorizationProvider.SetPermissions"/>来创建权限列表，所以，提供者(providers)不能使用依赖注入
        /// </remarks>
        public static IReadOnlyList<Permission> GetAllPermissions(params AuthorizationProvider[] authorizationProviders)
        {
            return new InternalPermissionFinder(authorizationProviders).GetAllPermissions();
        }

        /// <summary>
        /// 权限检查器
        /// </summary>
        internal class InternalPermissionFinder : PermissionDefinitionContextBase
        {
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="authorizationProviders"></param>
            public InternalPermissionFinder(params AuthorizationProvider[] authorizationProviders)
            {
                foreach (var provider in authorizationProviders)
                {
                    provider.SetPermissions(this);
                }

                Permissions.AddAllPermissions();
            }

            /// <summary>
            /// 获取权限列表
            /// </summary>
            /// <returns></returns>
            public IReadOnlyList<Permission> GetAllPermissions()
            {
                return Permissions.Values.ToImmutableList();
            }
        }
    }
}