using Abp.MultiTenancy;

namespace Abp.Zero.EntityFramework
{
    /// <summary>
    /// 多商户种子接口
    /// </summary>
    public interface IMultiTenantSeed
    {
        /// <summary>
        /// 商户对象
        /// </summary>
        AbpTenantBase Tenant { get; set; }
    }
}