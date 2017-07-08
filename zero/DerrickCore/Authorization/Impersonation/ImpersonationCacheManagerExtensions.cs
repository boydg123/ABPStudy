using Abp.Runtime.Caching;

namespace Derrick.Authorization.Impersonation
{
    /// <summary>
    /// 模拟缓存管理扩展
    /// </summary>
    public static class ImpersonationCacheManagerExtensions
    {
        /// <summary>
        /// 获取模拟缓存
        /// </summary>
        /// <param name="cacheManager">缓存管理器</param>
        /// <returns></returns>
        public static ITypedCache<string, ImpersonationCacheItem> GetImpersonationCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, ImpersonationCacheItem>(ImpersonationCacheItem.CacheName);
        }
    }
}