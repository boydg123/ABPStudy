using Abp.MultiTenancy;

namespace Abp.Runtime.Session
{
    /// <summary>
    /// Implements null object pattern for <see cref="IAbpSession"/>.
    /// 为<see cref="IAbpSession"/>实现null对象模式
    /// </summary>
    public class NullAbpSession : IAbpSession
    {
        /// <summary>
        /// Singleton instance.
        /// 单例
        /// </summary>
        public static NullAbpSession Instance { get { return SingletonInstance; } }
        private static readonly NullAbpSession SingletonInstance = new NullAbpSession();

        /// <summary>
        /// 获取当前用户编号，它是可空的，如果用户没有登录
        /// </summary>
        public long? UserId { get { return null; } }

        /// <summary>
        /// 获取当前租户ID,这个租户ID应该是<see cref="UserId"/>的租户ID,它可为null如果给定的<see cref="UserId"/>是宿主用户，或者用户没有登录
        /// </summary>
        public int? TenantId { get { return null; } }

        /// <summary>
        /// 表示多租户双方中的一方
        /// </summary>
        public MultiTenancySides MultiTenancySide { get { return MultiTenancySides.Tenant; } }

        /// <summary>
        /// 虚拟的用户ID，如果用户正在执行方法代表(<see cref="UserId"/>)则应该填充它
        /// </summary>
        public long? ImpersonatorUserId { get { return null; } }

        /// <summary>
        /// 虚拟的租户ID,如果一个用户使用<see cref="UserId"/>代表使用 <see cref="ImpersonatorUserId"/>执行方法，则应该填充它
        /// </summary>
        public int? ImpersonatorTenantId { get { return null; } }

        /// <summary>
        /// 构造函数
        /// </summary>
        private NullAbpSession()
        {

        }
    }
}