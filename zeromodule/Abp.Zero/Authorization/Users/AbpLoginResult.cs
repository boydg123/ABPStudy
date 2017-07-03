using System.Security.Claims;
using Abp.MultiTenancy;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// ABP登录结果
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    public class AbpLoginResult<TTenant, TUser>
        where TTenant : AbpTenant<TUser>
        where TUser : AbpUser<TUser>
    {
        /// <summary>
        /// 登录结果类型
        /// </summary>
        public AbpLoginResultType Result { get; private set; }
        /// <summary>
        /// 商户
        /// </summary>
        public TTenant Tenant { get; private set; }
        /// <summary>
        /// 用户
        /// </summary>
        public TUser User { get; private set; }
        /// <summary>
        /// 用户的标识
        /// </summary>
        public ClaimsIdentity Identity { get; private set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="result">登录结果类型</param>
        /// <param name="tenant">商户</param>
        /// <param name="user">用户</param>
        public AbpLoginResult(AbpLoginResultType result, TTenant tenant = null, TUser user = null)
        {
            Result = result;
            Tenant = tenant;
            User = user;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tenant">商户</param>
        /// <param name="user">用户</param>
        /// <param name="identity">用户的标识</param>
        public AbpLoginResult(TTenant tenant, TUser user, ClaimsIdentity identity)
            : this(AbpLoginResultType.Success, tenant)
        {
            User = user;
            Identity = identity;
        }
    }
}