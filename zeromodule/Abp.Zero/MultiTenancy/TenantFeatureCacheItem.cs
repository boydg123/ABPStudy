using System;
using System.Collections.Generic;

namespace Abp.MultiTenancy
{
    /// <summary>
    /// Used to store features of a Tenant in the cache.
    /// 用于在缓存中存储商户的功能
    /// </summary>
    [Serializable]
    public class TenantFeatureCacheItem
    {
        /// <summary>
        /// The cache store name.
        /// 缓存存储的名称
        /// </summary>
        public const string CacheStoreName = "AbpZeroTenantFeatures";

        /// <summary>
        /// Edition of the tenant.
        /// 商户的版本
        /// </summary>
        public int? EditionId { get; set; }

        /// <summary>
        /// Feature values.
        /// 功能值字典
        /// </summary>
        public IDictionary<string, string> FeatureValues { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TenantFeatureCacheItem()
        {
            FeatureValues = new Dictionary<string, string>();
        }
    }
}