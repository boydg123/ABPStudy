namespace Abp.Runtime.Caching
{
    /// <summary>
    /// Extension methods for <see cref="ICacheManager"/>.
    /// <see cref="ICacheManager"/>的扩展方法
    /// </summary>
    public static class CacheManagerExtensions
    {
        /// <summary>
        /// 获取类型缓存
        /// </summary>
        /// <typeparam name="TKey">缓存Key的类型</typeparam>
        /// <typeparam name="TValue">缓存值的类型</typeparam>
        /// <param name="cacheManager">缓存管理器</param>
        /// <param name="name">缓存名称</param>
        /// <returns></returns>
        public static ITypedCache<TKey, TValue> GetCache<TKey, TValue>(this ICacheManager cacheManager, string name)
        {
            return cacheManager.GetCache(name).AsTyped<TKey, TValue>();
        }
    }
}