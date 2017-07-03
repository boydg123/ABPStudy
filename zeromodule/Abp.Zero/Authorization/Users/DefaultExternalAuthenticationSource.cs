using System.Threading.Tasks;
using Abp.MultiTenancy;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// This is a helper base class to easily update <see cref="IExternalAuthenticationSource{TTenant,TUser}"/>.Implements some methods as default but you can override all methods.
    /// 这是帮助<see cref="IExternalAuthenticationSource{TTenant,TUser}"/>轻松修改的基类，实现了一些默认方法，但可以重写所有方法
    /// </summary>
    /// <typeparam name="TTenant">商户类型</typeparam>
    /// <typeparam name="TUser">用户类型</typeparam>
    public abstract class DefaultExternalAuthenticationSource<TTenant, TUser> : IExternalAuthenticationSource<TTenant, TUser>
        where TTenant : AbpTenant<TUser>
        where TUser : AbpUser<TUser>, new()
    {
        /// <summary>
        /// 认证源的唯一名称。源名称通过<see cref="AbpUser{TTenant,TUser}.AuthenticationSource"/>设置，如果用户通过这个授权源授权。
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// 用于尝试通过该源验证用户
        /// </summary>
        /// <param name="userNameOrEmailAddress">用户名或邮箱地址</param>
        /// <param name="plainPassword">用户的普通密码</param>
        /// <param name="tenant">用户的租户或NULL（如果用户是主机用户）</param>
        /// <returns>true，表示此应用程序已由该源进行身份验证。</returns>
        public abstract Task<bool> TryAuthenticateAsync(string userNameOrEmailAddress, string plainPassword, TTenant tenant);

        /// <summary>
        /// 此方法是由该源验证的用户，该源尚未存在。因此，源应该创建用户和填充属性。
        /// </summary>
        /// <param name="userNameOrEmailAddress">用户名或邮箱地址</param>
        /// <param name="tenant">用户的租户或NULL（如果用户是主机用户）</param>
        /// <returns>新创建的用户</returns>
        public virtual Task<TUser> CreateUserAsync(string userNameOrEmailAddress, TTenant tenant)
        {
            return Task.FromResult(
                new TUser
                {
                    UserName = userNameOrEmailAddress,
                    Name = userNameOrEmailAddress,
                    Surname = userNameOrEmailAddress,
                    EmailAddress = userNameOrEmailAddress,
                    IsEmailConfirmed = true,
                    IsActive = true
                });
        }

        /// <summary>
        /// 此方法在通过此源验证已存在的用户之后被调用，通过此源它能修改用户的一些属性
        /// </summary>
        /// <param name="user">能被修改的用户</param>
        /// <param name="tenant">用户的租户或NULL（如果用户是主机用户）</param>
        public virtual Task UpdateUserAsync(TUser user, TTenant tenant)
        {
            return Task.FromResult(0);
        }
    }
}