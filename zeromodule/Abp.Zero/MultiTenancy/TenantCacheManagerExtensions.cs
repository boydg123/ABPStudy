using Abp.Runtime.Caching;

namespace Abp.MultiTenancy
{
    /// <summary>
    /// 商户缓存管理的扩展
    /// </summary>
    public static class TenantCacheManagerExtensions
    {
        /// <summary>
        /// 获取商户缓存
        /// </summary>
        /// <param name="cacheManager">缓存管理对象</param>
        /// <returns></returns>
        public static ITypedCache<int, TenantCacheItem> GetTenantCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<int, TenantCacheItem>(TenantCacheItem.CacheName);
        }
        /// <summary>
        /// 通过名称缓存获取商户
        /// </summary>
        /// <param name="cacheManager">缓存管理对象</param>
        /// <returns></returns>
        public static ITypedCache<string, int?> GetTenantByNameCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, int?>(TenantCacheItem.ByNameCacheName);
        }
    }
}