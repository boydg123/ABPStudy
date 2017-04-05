using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Localization;
using Abp.Threading;

namespace Abp.Authorization
{
    /// <summary>
    /// Extension methods for <see cref="IPermissionChecker"/>
    /// <see cref="IPermissionChecker"/>的扩展方法
    /// </summary>
    public static class PermissionCheckerExtensions
    {
        /// <summary>
        /// Checks if current user is granted for a permission.
        /// 检查当前用户是否被授予给定的权限
        /// </summary>
        /// <param name="permissionChecker">Permission checker / 权限检测器</param>
        /// <param name="permissionName">Name of the permission / 权限名称</param>
        public static bool IsGranted(this IPermissionChecker permissionChecker, string permissionName)
        {
            return AsyncHelper.RunSync(() => permissionChecker.IsGrantedAsync(permissionName));
        }

        /// <summary>
        /// Checks if a user is granted for a permission.
        /// 检查指定用户是否被授予给定的权限
        /// </summary>
        /// <param name="permissionChecker">Permission checker / 权限检测器</param>
        /// <param name="user">User to check / 检查的用户</param>
        /// <param name="permissionName">Name of the permission / 权限名称</param>
        public static bool IsGranted(this IPermissionChecker permissionChecker, UserIdentifier user, string permissionName)
        {
            return AsyncHelper.RunSync(() => permissionChecker.IsGrantedAsync(user, permissionName));
        }

        /// <summary>
        /// Checks if given user is granted for given permission.
        /// 检查给定用户是否给予给定权限
        /// </summary>
        /// <param name="permissionChecker">Permission checker / 权限检测器</param>
        /// <param name="user">User / 检查的用户</param>
        /// <param name="requiresAll">True, to require all given permissions are granted. False, to require one or more. / true,需要授予所有给定的权限，false，需要一个或更多</param>
        /// <param name="permissionNames">Name of the permissions / 权限的名称</param>
        public static bool IsGranted(this IPermissionChecker permissionChecker, UserIdentifier user, bool requiresAll, params string[] permissionNames)
        {
            return AsyncHelper.RunSync(() => IsGrantedAsync(permissionChecker, user, requiresAll, permissionNames));
        }

        /// <summary>
        /// Checks if given user is granted for given permission.
        /// 检查给定用户是否给予给定权限
        /// </summary>
        /// <param name="permissionChecker">Permission checker / 权限检测器</param>
        /// <param name="user">User / 检查的用户</param>
        /// <param name="requiresAll">True, to require all given permissions are granted. False, to require one or more. / true,需要授予所有给定的权限，false，需要一个或更多</param>
        /// <param name="permissionNames">Name of the permissions / 权限的名称</param>
        public static async Task<bool> IsGrantedAsync(this IPermissionChecker permissionChecker, UserIdentifier user, bool requiresAll, params string[] permissionNames)
        {
            if (permissionNames.IsNullOrEmpty())
            {
                return true;
            }

            if (requiresAll)
            {
                foreach (var permissionName in permissionNames)
                {
                    if (!(await permissionChecker.IsGrantedAsync(user, permissionName)))
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                foreach (var permissionName in permissionNames)
                {
                    if (await permissionChecker.IsGrantedAsync(user, permissionName))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Checks if current user is granted for given permission.
        /// 检查当前用户是否给予给定权限
        /// </summary>
        /// <param name="permissionChecker">Permission checker / 权限检测器</param>
        /// <param name="requiresAll">True, to require all given permissions are granted. False, to require one or more. / true,需要授予所有给定的权限，false，需要一个或更多</param>
        /// <param name="permissionNames">Name of the permissions / 权限的名称</param>
        public static bool IsGranted(this IPermissionChecker permissionChecker, bool requiresAll, params string[] permissionNames)
        {
            return AsyncHelper.RunSync(() => IsGrantedAsync(permissionChecker, requiresAll, permissionNames));
        }

        /// <summary>
        /// Checks if current user is granted for given permission.
        /// 检查当前用户是否给予给定权限
        /// </summary>
        /// <param name="permissionChecker">Permission checker / 权限检测器</param>
        /// <param name="requiresAll">True, to require all given permissions are granted. False, to require one or more. / true,需要授予所有给定的权限，false，需要一个或更多</param>
        /// <param name="permissionNames">Name of the permissions / 权限的名称</param>
        public static async Task<bool> IsGrantedAsync(this IPermissionChecker permissionChecker, bool requiresAll, params string[] permissionNames)
        {
            if (permissionNames.IsNullOrEmpty())
            {
                return true;
            }

            if (requiresAll)
            {
                foreach (var permissionName in permissionNames)
                {
                    if (!(await permissionChecker.IsGrantedAsync(permissionName)))
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                foreach (var permissionName in permissionNames)
                {
                    if (await permissionChecker.IsGrantedAsync(permissionName))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Authorizes current user for given permission or permissions,throws <see cref="AbpAuthorizationException"/> if not authorized.User it authorized if any of the <see cref="permissionNames"/> are granted.
        /// 当前用户是否被授予给定的权限,如果未授权，抛出异常 <see cref="AbpAuthorizationException"/> ,如果<see cref="permissionNames"/>中的任一个被授予用户，那么他就是授权的
        /// </summary>
        /// <param name="permissionChecker">Permission checker / 权限检测器</param>
        /// <param name="permissionNames">Name of the permissions to authorize / 权限名称</param>
        /// <exception cref="AbpAuthorizationException">Throws authorization exception if / 可能抛出的异常</exception>
        public static void Authorize(this IPermissionChecker permissionChecker, params string[] permissionNames)
        {
            Authorize(permissionChecker, false, permissionNames);
        }

        /// <summary>
        /// Authorizes current user for given permission or permissions,throws <see cref="AbpAuthorizationException"/> if not authorized.User it authorized if any of the <see cref="permissionNames"/> are granted.
        /// 当前用户是否被授予给定的权限，如果未授权，抛出异常 <see cref="AbpAuthorizationException"/>如果<see cref="permissionNames"/>中的任一个被授予用户，那么他就是授权的
        /// </summary>
        /// <param name="permissionChecker">Permission checker权限检测器</param>
        /// <param name="requireAll">
        /// If this is set to true, all of the <see cref="permissionNames"/> must be granted.
        /// 如果它设置为true，所有的<see cref="permissionNames"/>都必须被授予
        /// If it's false, at least one of the <see cref="permissionNames"/> must be granted.
        /// 如果它设置为false，<see cref="permissionNames"/>中至少有一个权限必须被授予
        /// </param>
        /// <param name="permissionNames">Name of the permissions to authorize / 权限名称</param>
        /// <exception cref="AbpAuthorizationException">Throws authorization exception if / 可能抛出此异常</exception>
        public static void Authorize(this IPermissionChecker permissionChecker, bool requireAll, params string[] permissionNames)
        {
            AsyncHelper.RunSync(() => AuthorizeAsync(permissionChecker, requireAll, permissionNames));
        }

        /// <summary>
        /// Authorizes current user for given permission or permissions,throws <see cref="AbpAuthorizationException"/> if not authorized.User it authorized if any of the <see cref="permissionNames"/> are granted.
        /// 当前用户是否被授予给定的权限，如果未授权，抛出异常 <see cref="AbpAuthorizationException"/>，如果<see cref="permissionNames"/>中的任一个被授予用户，那么他就是授权的
        /// </summary>
        /// <param name="permissionChecker">Permission checker / 权限检测器</param>
        /// <param name="permissionNames">Name of the permissions to authorize / 权限名称</param>
        /// <exception cref="AbpAuthorizationException">Throws authorization exception if / 可能抛出此异常</exception>
        public static Task AuthorizeAsync(this IPermissionChecker permissionChecker, params string[] permissionNames)
        {
            return AuthorizeAsync(permissionChecker, false, permissionNames);
        }

        /// <summary>
        /// Authorizes current user for given permission or permissions,throws <see cref="AbpAuthorizationException"/> if not authorized.
        /// 当前用户是否被授予给定的权限,如果未授权，抛出异常 <see cref="AbpAuthorizationException"/>
        /// </summary>
        /// <param name="permissionChecker">Permission checker / 权限检测器</param>
        /// <param name="requireAll">
        /// If this is set to true, all of the <see cref="permissionNames"/> must be granted.
        /// 如果它设置为true，所有的<see cref="permissionNames"/>都必须被授予
        /// If it's false, at least one of the <see cref="permissionNames"/> must be granted.
        /// 如果它设置为false，<see cref="permissionNames"/>中至少有一个权限必须被授予
        /// </param>
        /// <param name="permissionNames">Name of the permissions to authorize / 权限名称</param>
        /// <exception cref="AbpAuthorizationException">Throws authorization exception if / 可能抛出此异常</exception>
        public static async Task AuthorizeAsync(this IPermissionChecker permissionChecker, bool requireAll, params string[] permissionNames)
        {
            if (await IsGrantedAsync(permissionChecker, requireAll, permissionNames))
            {
                return;
            }

            var localizedPermissionNames = LocalizePermissionNames(permissionChecker, permissionNames);

            if (requireAll)
            {
                throw new AbpAuthorizationException(
                    string.Format(
                        L(
                            permissionChecker,
                            "AllOfThesePermissionsMustBeGranted",
                            "Required permissions are not granted. All of these permissions must be granted: {0}"
                        ),
                        string.Join(", ", localizedPermissionNames)
                    )
                );
            }
            else
            {
                throw new AbpAuthorizationException(
                    string.Format(
                        L(
                            permissionChecker,
                            "AtLeastOneOfThesePermissionsMustBeGranted",
                            "Required permissions are not granted. At least one of these permissions must be granted: {0}"
                        ),
                        string.Join(", ", localizedPermissionNames)
                    )
                );
            }
        }

        /// <summary>
        /// 获取本地化资源字符串
        /// </summary>
        /// <param name="permissionChecker">权限检查器</param>
        /// <param name="name">权限名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string L(IPermissionChecker permissionChecker, string name, string defaultValue)
        {
            if (!(permissionChecker is IIocManagerAccessor))
            {
                return defaultValue;
            }

            var iocManager = (permissionChecker as IIocManagerAccessor).IocManager;
            using (var localizationManager = iocManager.ResolveAsDisposable<ILocalizationManager>())
            {
                return localizationManager.Object.GetString(AbpConsts.LocalizationSourceName, name);
            }
        }

        /// <summary>
        /// 本地化权限名称
        /// </summary>
        /// <param name="permissionChecker">权限检查器</param>
        /// <param name="permissionNames">权限名称</param>
        /// <returns></returns>
        public static string[] LocalizePermissionNames(IPermissionChecker permissionChecker, string[] permissionNames)
        {
            if (!(permissionChecker is IIocManagerAccessor))
            {
                return permissionNames;
            }

            var iocManager = (permissionChecker as IIocManagerAccessor).IocManager;
            using (var localizationContext = iocManager.ResolveAsDisposable<ILocalizationContext>())
            {
                using (var permissionManager = iocManager.ResolveAsDisposable<IPermissionManager>())
                {
                    return permissionNames.Select(permissionName =>
                    {
                        var permission = permissionManager.Object.GetPermissionOrNull(permissionName);
                        return permission == null
                            ? permissionName
                            : permission.DisplayName.Localize(localizationContext.Object);
                    }).ToArray();
                }
            }
        }
    }
}