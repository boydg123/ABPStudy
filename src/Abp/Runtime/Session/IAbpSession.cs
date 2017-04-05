using Abp.MultiTenancy;

namespace Abp.Runtime.Session
{
    /// <summary>
    /// Defines some session information that can be useful for applications.
    /// 为应用定义一些有用的会话信息
    /// </summary>
    public interface IAbpSession
    {
        /// <summary>
        /// Gets current UserId or null.It can be null if no user logged in.
        /// 获取当前用户编号，它是可空的，如果用户没有登录
        /// </summary>
        long? UserId { get; }

        /// <summary>
        /// Gets current TenantId or null.
        /// 获取当前租户ID
        /// This TenantId should be the TenantId of the <see cref="UserId"/>.
        /// 这个租户ID应该是<see cref="UserId"/>的租户ID
        /// It can be null if given <see cref="UserId"/> is a host user or no user logged in.
        /// 它可为null如果给定的<see cref="UserId"/>是宿主用户，或者用户没有登录
        /// </summary>
        int? TenantId { get; }

        /// <summary>
        /// Gets current multi-tenancy side.
        /// 获取当前多租户双方的一方
        /// </summary>
        MultiTenancySides MultiTenancySide { get; }

        /// <summary>
        /// UserId of the impersonator.This is filled if a user is performing actions behalf of the <see cref="UserId"/>.
        /// 虚拟的用户ID，如果用户正在执行方法代表(<see cref="UserId"/>)则应该填充它
        /// </summary>
        long? ImpersonatorUserId { get; }

        /// <summary>
        /// TenantId of the impersonator.
        /// 虚拟的租户ID
        /// This is filled if a user with <see cref="ImpersonatorUserId"/> performing actions behalf of the <see cref="UserId"/>.
        /// 如果一个用户使用<see cref="UserId"/>代表使用 <see cref="ImpersonatorUserId"/>执行方法，则应该填充它
        /// </summary>
        int? ImpersonatorTenantId { get; }
    }
}
