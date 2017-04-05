using System;

namespace Abp.Runtime.Session
{
    /// <summary>
    /// Extension methods for <see cref="IAbpSession"/>.
    /// <see cref="IAbpSession"/>的扩展方法
    /// </summary>
    public static class AbpSessionExtensions
    {
        /// <summary>
        /// Gets current User's Id.Throws <see cref="AbpException"/> if <see cref="IAbpSession.UserId"/> is null.
        /// 获取当前用户ID，如果<see cref="IAbpSession.UserId"/>为null则抛出异常<see cref="AbpException"/>
        /// </summary>
        /// <param name="session">Session object.</param>
        /// <returns>Current User's Id.</returns>
        public static long GetUserId(this IAbpSession session)
        {
            if (!session.UserId.HasValue)
            {
                throw new AbpException("Session.UserId is null! Probably, user is not logged in.");
            }

            return session.UserId.Value;
        }

        /// <summary>
        /// Gets current Tenant's Id.Throws <see cref="AbpException"/> if <see cref="IAbpSession.TenantId"/> is null.
        /// 获取当前用户ID，如果<see cref="IAbpSession.UserId"/>为null则抛出异常<see cref="AbpException"/>
        /// </summary>
        /// <param name="session">Session object.</param>
        /// <returns>Current Tenant's Id.</returns>
        /// <exception cref="AbpException"></exception>
        public static int GetTenantId(this IAbpSession session)
        {
            if (!session.TenantId.HasValue)
            {
                throw new AbpException("Session.TenantId is null! Possible problems: No user logged in or current logged in user in a host user (TenantId is always null for host users).");
            }

            return session.TenantId.Value;
        }

        /// <summary>
        /// Creates <see cref="UserIdentifier"/> from given session.Returns null if <see cref="IAbpSession.UserId"/> is null.
        /// 为给定session创建<see cref="UserIdentifier"/>,如果<see cref="IAbpSession.UserId"/>为null则返回null
        /// </summary>
        /// <param name="session">The session.</param>
        public static UserIdentifier ToUserIdentifier(this IAbpSession session)
        {
            return session.UserId == null
                ? null
                : new UserIdentifier(session.TenantId, session.GetUserId());
        }
    }
}