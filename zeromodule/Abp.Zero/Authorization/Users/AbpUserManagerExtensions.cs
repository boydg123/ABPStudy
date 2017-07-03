using System;
using Abp.Authorization.Roles;
using Abp.MultiTenancy;
using Abp.Threading;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Extension methods for <see cref="AbpUserManager{TRole,TUser}"/>.
    /// <see cref="AbpUserManager{TRole,TUser}"/>的扩展方法
    /// </summary>
    public static class AbpUserManagerExtensions
    {
        /// <summary>
        /// Check whether a user is granted for a permission.
        /// 检查用户是否授权
        /// </summary>
        /// <param name="manager">User manager / 用户管理</param>
        /// <param name="userId">User id / 用户ID</param>
        /// <param name="permissionName">Permission name / 权限名称</param>
        public static bool IsGranted<TRole, TUser>(AbpUserManager<TRole, TUser> manager, long userId, string permissionName)
            where TRole : AbpRole<TUser>, new()
            where TUser : AbpUser<TUser>
        {
            if (manager == null)
            {
                throw new ArgumentNullException(nameof(manager));
            }

            return AsyncHelper.RunSync(() => manager.IsGrantedAsync(userId, permissionName));
        }

        //public static AbpUserManager<TRole, TUser> Login<TRole, TUser>(AbpUserManager<TRole, TUser> manager, string userNameOrEmailAddress, string plainPassword, string tenancyName = null)
        //    where TRole : AbpRole<TUser>, new()
        //    where TUser : AbpUser<TUser>
        //{
        //    if (manager == null)
        //    {
        //        throw new ArgumentNullException(nameof(manager));
        //    }

        //    return AsyncHelper.RunSync(() => manager.LoginAsync(userNameOrEmailAddress, plainPassword, tenancyName));
        //}
    }
}