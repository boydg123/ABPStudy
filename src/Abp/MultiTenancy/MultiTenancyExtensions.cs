using Abp.Domain.Entities;

namespace Abp.MultiTenancy
{
    /// <summary>
    /// Extension methods for multi-tenancy.
    /// 多租户扩展方法
    /// </summary>
    public static class MultiTenancyExtensions
    {
        /// <summary>
        /// Gets multi-tenancy side (<see cref="MultiTenancySides"/>) of an object that implements <see cref="IMayHaveTenant"/>.
        /// 获取多租户方 (<see cref="MultiTenancySides"/>)，它实现了接口 <see cref="IMayHaveTenant"/>.
        /// </summary>
        /// <param name="obj">The object / 表示多租户方的对象</param>
        public static MultiTenancySides GetMultiTenancySide(this IMayHaveTenant obj)
        {
            return obj.TenantId.HasValue
                ? MultiTenancySides.Tenant
                : MultiTenancySides.Host;
        }
    }
}