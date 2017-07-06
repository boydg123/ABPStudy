using Abp.Authorization.Users;
using Castle.Core.Logging;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace Abp.Owin
{
    /// <summary>
    /// Owin 用户Token 提供访问器
    /// </summary>
    public class OwinUserTokenProviderAccessor : IUserTokenProviderAccessor
    {
        /// <summary>
        /// 日志引用
        /// </summary>
        public ILogger Logger { get; set; }
        /// <summary>
        /// 数据保护提供者
        /// </summary>
        public IDataProtectionProvider DataProtectionProvider { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public OwinUserTokenProviderAccessor()
        {
            Logger = NullLogger.Instance;
        }
        /// <summary>
        /// 获取用户Token提供器或Null
        /// </summary>
        /// <typeparam name="TUser">用户对象</typeparam>
        /// <returns></returns>
        public IUserTokenProvider<TUser, long> GetUserTokenProviderOrNull<TUser>()
            where TUser : AbpUser<TUser>
        {
            if (DataProtectionProvider == null)
            {
                Logger.Debug("DataProtectionProvider has not been set yet.");
                return null;
            }

            return new DataProtectorTokenProvider<TUser, long>(DataProtectionProvider.Create("ASP.NET Identity"));
        }
    }
}