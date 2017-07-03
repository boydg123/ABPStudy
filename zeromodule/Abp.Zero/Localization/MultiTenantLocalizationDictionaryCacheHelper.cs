using System.Collections.Generic;
using Abp.Runtime.Caching;

namespace Abp.Localization
{
    /// <summary>
    /// A helper to implement localization cache.
    /// 实现本地化缓存的帮助类
    /// </summary>
    public static class MultiTenantLocalizationDictionaryCacheHelper
    {
        /// <summary>
        /// 缓存Key名称.
        /// </summary>
        public const string CacheName = "AbpZeroMultiTenantLocalizationDictionaryCache";
        /// <summary>
        /// 获取多商户本地化缓存字典
        /// </summary>
        /// <param name="cacheManager"></param>
        /// <returns></returns>
        public static ITypedCache<string, Dictionary<string, string>> GetMultiTenantLocalizationDictionaryCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache(CacheName).AsTyped<string, Dictionary<string, string>>();
        }
        /// <summary>
        /// 计算缓存Key
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <param name="sourceName">源名称</param>
        /// <param name="languageName">语言名称</param>
        /// <returns></returns>
        public static string CalculateCacheKey(int? tenantId, string sourceName, string languageName)
        {
            return sourceName + "#" + languageName + "#" + (tenantId ?? 0);
        }
    }
}