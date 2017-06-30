using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Threading;

namespace Abp.Authorization
{
    /// <summary>
    /// ABP登录管理扩展
    /// </summary>
    public static class AbpLogInManagerExtensions
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <typeparam name="TTenant">商户</typeparam>
        /// <typeparam name="TRole">角色</typeparam>
        /// <typeparam name="TUser">用户</typeparam>
        /// <param name="logInManager">登录管理器</param>
        /// <param name="userNameOrEmailAddress">用户名或邮箱</param>
        /// <param name="plainPassword">密码</param>
        /// <param name="tenancyName">商户名称</param>
        /// <param name="shouldLockout">是否锁定</param>
        /// <returns></returns>
        public static AbpLoginResult<TTenant, TUser> Login<TTenant, TRole, TUser>(
            this AbpLogInManager<TTenant, TRole, TUser> logInManager, 
            string userNameOrEmailAddress, 
            string plainPassword, 
            string tenancyName = null, 
            bool shouldLockout = true)
                where TTenant : AbpTenant<TUser>
                where TRole : AbpRole<TUser>, new()
                where TUser : AbpUser<TUser>
        {
            return AsyncHelper.RunSync(
                () => logInManager.LoginAsync(
                    userNameOrEmailAddress,
                    plainPassword,
                    tenancyName,
                    shouldLockout
                )
            );
        }
    }
}
