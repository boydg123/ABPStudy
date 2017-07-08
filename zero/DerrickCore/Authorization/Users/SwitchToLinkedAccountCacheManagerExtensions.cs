using Abp.Runtime.Caching;

namespace Derrick.Authorization.Users
{
    /// <summary>
    /// 切换到链接帐号的缓存管理扩展
    /// </summary>
    public static class SwitchToLinkedAccountCacheManagerExtensions
    {
        /// <summary>
        /// 获取切换连接账户缓存
        /// </summary>
        /// <param name="cacheManager">缓存管理器</param>
        /// <returns></returns>
        public static ITypedCache<string, SwitchToLinkedAccountCacheItem> GetSwitchToLinkedAccountCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, SwitchToLinkedAccountCacheItem>(SwitchToLinkedAccountCacheItem.CacheName);
        }
    }
}