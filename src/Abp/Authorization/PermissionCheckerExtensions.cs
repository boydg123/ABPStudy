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
    /// <see cref="IPermissionChecker"/>����չ����
    /// </summary>
    public static class PermissionCheckerExtensions
    {
        /// <summary>
        /// Checks if current user is granted for a permission.
        /// ��鵱ǰ�û��Ƿ����������Ȩ��
        /// </summary>
        /// <param name="permissionChecker">Permission checker / Ȩ�޼����</param>
        /// <param name="permissionName">Name of the permission / Ȩ������</param>
        public static bool IsGranted(this IPermissionChecker permissionChecker, string permissionName)
        {
            return AsyncHelper.RunSync(() => permissionChecker.IsGrantedAsync(permissionName));
        }

        /// <summary>
        /// Checks if a user is granted for a permission.
        /// ���ָ���û��Ƿ����������Ȩ��
        /// </summary>
        /// <param name="permissionChecker">Permission checker / Ȩ�޼����</param>
        /// <param name="user">User to check / �����û�</param>
        /// <param name="permissionName">Name of the permission / Ȩ������</param>
        public static bool IsGranted(this IPermissionChecker permissionChecker, UserIdentifier user, string permissionName)
        {
            return AsyncHelper.RunSync(() => permissionChecker.IsGrantedAsync(user, permissionName));
        }

        /// <summary>
        /// Checks if given user is granted for given permission.
        /// �������û��Ƿ�������Ȩ��
        /// </summary>
        /// <param name="permissionChecker">Permission checker / Ȩ�޼����</param>
        /// <param name="user">User / �����û�</param>
        /// <param name="requiresAll">True, to require all given permissions are granted. False, to require one or more. / true,��Ҫ�������и�����Ȩ�ޣ�false����Ҫһ�������</param>
        /// <param name="permissionNames">Name of the permissions / Ȩ�޵�����</param>
        public static bool IsGranted(this IPermissionChecker permissionChecker, UserIdentifier user, bool requiresAll, params string[] permissionNames)
        {
            return AsyncHelper.RunSync(() => IsGrantedAsync(permissionChecker, user, requiresAll, permissionNames));
        }

        /// <summary>
        /// Checks if given user is granted for given permission.
        /// �������û��Ƿ�������Ȩ��
        /// </summary>
        /// <param name="permissionChecker">Permission checker / Ȩ�޼����</param>
        /// <param name="user">User / �����û�</param>
        /// <param name="requiresAll">True, to require all given permissions are granted. False, to require one or more. / true,��Ҫ�������и�����Ȩ�ޣ�false����Ҫһ�������</param>
        /// <param name="permissionNames">Name of the permissions / Ȩ�޵�����</param>
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
        /// ��鵱ǰ�û��Ƿ�������Ȩ��
        /// </summary>
        /// <param name="permissionChecker">Permission checker / Ȩ�޼����</param>
        /// <param name="requiresAll">True, to require all given permissions are granted. False, to require one or more. / true,��Ҫ�������и�����Ȩ�ޣ�false����Ҫһ�������</param>
        /// <param name="permissionNames">Name of the permissions / Ȩ�޵�����</param>
        public static bool IsGranted(this IPermissionChecker permissionChecker, bool requiresAll, params string[] permissionNames)
        {
            return AsyncHelper.RunSync(() => IsGrantedAsync(permissionChecker, requiresAll, permissionNames));
        }

        /// <summary>
        /// Checks if current user is granted for given permission.
        /// ��鵱ǰ�û��Ƿ�������Ȩ��
        /// </summary>
        /// <param name="permissionChecker">Permission checker / Ȩ�޼����</param>
        /// <param name="requiresAll">True, to require all given permissions are granted. False, to require one or more. / true,��Ҫ�������и�����Ȩ�ޣ�false����Ҫһ�������</param>
        /// <param name="permissionNames">Name of the permissions / Ȩ�޵�����</param>
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
        /// ��ǰ�û��Ƿ����������Ȩ��,���δ��Ȩ���׳��쳣 <see cref="AbpAuthorizationException"/> ,���<see cref="permissionNames"/>�е���һ���������û�����ô��������Ȩ��
        /// </summary>
        /// <param name="permissionChecker">Permission checker / Ȩ�޼����</param>
        /// <param name="permissionNames">Name of the permissions to authorize / Ȩ������</param>
        /// <exception cref="AbpAuthorizationException">Throws authorization exception if / �����׳����쳣</exception>
        public static void Authorize(this IPermissionChecker permissionChecker, params string[] permissionNames)
        {
            Authorize(permissionChecker, false, permissionNames);
        }

        /// <summary>
        /// Authorizes current user for given permission or permissions,throws <see cref="AbpAuthorizationException"/> if not authorized.User it authorized if any of the <see cref="permissionNames"/> are granted.
        /// ��ǰ�û��Ƿ����������Ȩ�ޣ����δ��Ȩ���׳��쳣 <see cref="AbpAuthorizationException"/>���<see cref="permissionNames"/>�е���һ���������û�����ô��������Ȩ��
        /// </summary>
        /// <param name="permissionChecker">Permission checkerȨ�޼����</param>
        /// <param name="requireAll">
        /// If this is set to true, all of the <see cref="permissionNames"/> must be granted.
        /// ���������Ϊtrue�����е�<see cref="permissionNames"/>�����뱻����
        /// If it's false, at least one of the <see cref="permissionNames"/> must be granted.
        /// ���������Ϊfalse��<see cref="permissionNames"/>��������һ��Ȩ�ޱ��뱻����
        /// </param>
        /// <param name="permissionNames">Name of the permissions to authorize / Ȩ������</param>
        /// <exception cref="AbpAuthorizationException">Throws authorization exception if / �����׳����쳣</exception>
        public static void Authorize(this IPermissionChecker permissionChecker, bool requireAll, params string[] permissionNames)
        {
            AsyncHelper.RunSync(() => AuthorizeAsync(permissionChecker, requireAll, permissionNames));
        }

        /// <summary>
        /// Authorizes current user for given permission or permissions,throws <see cref="AbpAuthorizationException"/> if not authorized.User it authorized if any of the <see cref="permissionNames"/> are granted.
        /// ��ǰ�û��Ƿ����������Ȩ�ޣ����δ��Ȩ���׳��쳣 <see cref="AbpAuthorizationException"/>�����<see cref="permissionNames"/>�е���һ���������û�����ô��������Ȩ��
        /// </summary>
        /// <param name="permissionChecker">Permission checker / Ȩ�޼����</param>
        /// <param name="permissionNames">Name of the permissions to authorize / Ȩ������</param>
        /// <exception cref="AbpAuthorizationException">Throws authorization exception if / �����׳����쳣</exception>
        public static Task AuthorizeAsync(this IPermissionChecker permissionChecker, params string[] permissionNames)
        {
            return AuthorizeAsync(permissionChecker, false, permissionNames);
        }

        /// <summary>
        /// Authorizes current user for given permission or permissions,throws <see cref="AbpAuthorizationException"/> if not authorized.
        /// ��ǰ�û��Ƿ����������Ȩ��,���δ��Ȩ���׳��쳣 <see cref="AbpAuthorizationException"/>
        /// </summary>
        /// <param name="permissionChecker">Permission checker / Ȩ�޼����</param>
        /// <param name="requireAll">
        /// If this is set to true, all of the <see cref="permissionNames"/> must be granted.
        /// ���������Ϊtrue�����е�<see cref="permissionNames"/>�����뱻����
        /// If it's false, at least one of the <see cref="permissionNames"/> must be granted.
        /// ���������Ϊfalse��<see cref="permissionNames"/>��������һ��Ȩ�ޱ��뱻����
        /// </param>
        /// <param name="permissionNames">Name of the permissions to authorize / Ȩ������</param>
        /// <exception cref="AbpAuthorizationException">Throws authorization exception if / �����׳����쳣</exception>
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
        /// ��ȡ���ػ���Դ�ַ���
        /// </summary>
        /// <param name="permissionChecker">Ȩ�޼����</param>
        /// <param name="name">Ȩ������</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
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
        /// ���ػ�Ȩ������
        /// </summary>
        /// <param name="permissionChecker">Ȩ�޼����</param>
        /// <param name="permissionNames">Ȩ������</param>
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