using System;
using Abp.Authorization.Users;

namespace Abp.Runtime.Session
{
    /// <summary>
    /// ABP Session扩展
    /// </summary>
    public static class AbpSessionExtensions
    {
        /// <summary>
        /// 是否是用户
        /// </summary>
        /// <param name="session">ABP Session对象</param>
        /// <param name="user">ABP 用户</param>
        /// <returns></returns>
        public static bool IsUser(this IAbpSession session, AbpUserBase user)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return session.TenantId == user.TenantId && 
                session.UserId.HasValue && 
                session.UserId.Value == user.Id;
        }
    }
}
