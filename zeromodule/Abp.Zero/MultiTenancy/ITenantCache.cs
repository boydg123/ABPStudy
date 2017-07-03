namespace Abp.MultiTenancy
{
    /// <summary>
    /// 商户缓存
    /// </summary>
    public interface ITenantCache
    {
        /// <summary>
        /// 获取商户缓存
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <returns></returns>
        TenantCacheItem Get(int tenantId);
        /// <summary>
        /// 获取商户缓存
        /// </summary>
        /// <param name="tenancyName">商户名称</param>
        /// <returns></returns>
        TenantCacheItem Get(string tenancyName);
        /// <summary>
        /// 获取商户缓存或Null
        /// </summary>
        /// <param name="tenancyName">商户名称</param>
        /// <returns></returns>
        TenantCacheItem GetOrNull(string tenancyName);
        /// <summary>
        /// 获取商户缓存或Null
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <returns></returns>
        TenantCacheItem GetOrNull(int tenantId);
    }
}