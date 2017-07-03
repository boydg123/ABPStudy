using System.Threading.Tasks;
using Abp.MultiTenancy;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// Defines an authorization source to be used by <see cref="AbpUserManager{TRole,TUser}.LoginAsync"/> method.
    /// 定义一个授权源给<see cref="AbpUserManager{TRole,TUser}.LoginAsync"/>方法使用
    /// </summary>
    /// <typeparam name="TTenant">Tenant type</typeparam>
    /// <typeparam name="TUser">User type</typeparam>
    public interface IExternalAuthenticationSource<TTenant, TUser>
        where TTenant : AbpTenant<TUser>
        where TUser : AbpUser<TUser>
    {
        /// <summary>
        /// Unique name of the authentication source.This source name is set to <see cref="AbpUser{TTenant,TUser}.AuthenticationSource"/>if the user authenticated by this authentication source
        /// 认证源的唯一名称。源名称通过<see cref="AbpUser{TTenant,TUser}.AuthenticationSource"/>设置，如果用户通过这个授权源授权。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Used to try authenticate a user by this source.
        /// 用于尝试通过该源验证用户
        /// </summary>
        /// <param name="userNameOrEmailAddress">User name or email address / 用户名或邮箱地址</param>
        /// <param name="plainPassword">Plain password of the user / 用户的普通密码</param>
        /// <param name="tenant">Tenant of the user or null (if user is a host user) / 用户的租户或NULL（如果用户是主机用户）</param>
        /// <returns>True, indicates that this used has authenticated by this source / true，表示此应用程序已由该源进行身份验证。</returns>
        Task<bool> TryAuthenticateAsync(string userNameOrEmailAddress, string plainPassword, TTenant tenant);

        /// <summary>
        /// This method is a user authenticated by this source which does not exists yet.So, source should create the User and fill properties.
        /// 此方法是由该源验证的用户，该源尚未存在。因此，源应该创建用户和填充属性。
        /// </summary>
        /// <param name="userNameOrEmailAddress">User name or email address / 用户名或邮箱地址</param>
        /// <param name="tenant">Tenant of the user or null (if user is a host user) / 用户的租户或NULL（如果用户是主机用户）</param>
        /// <returns>Newly created user / 新创建的用户</returns>
        Task<TUser> CreateUserAsync(string userNameOrEmailAddress, TTenant tenant);

        /// <summary>
        /// This method is called after an existing user is authenticated by this source.It can be used to update some properties of the user by the source.
        /// 此方法在通过此源验证已存在的用户之后被调用，通过此源它能修改用户的一些属性
        /// </summary>
        /// <param name="user">The user that can be updated / 能被修改的用户</param>
        /// <param name="tenant">Tenant of the user or null (if user is a host user) / 用户的租户或NULL（如果用户是主机用户）</param>
        Task UpdateUserAsync(TUser user, TTenant tenant);
    }
}