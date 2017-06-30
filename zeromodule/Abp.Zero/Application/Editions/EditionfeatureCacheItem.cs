using System;
using System.Collections.Generic;

namespace Abp.Application.Editions
{
    /// <summary>
    /// 版本功能缓存项
    /// </summary>
    [Serializable]
    public class EditionfeatureCacheItem
    {
        /// <summary>
        /// 缓存存储Name
        /// </summary>
        public const string CacheStoreName = "AbpZeroEditionFeatures";
        /// <summary>
        /// 功能集合
        /// </summary>
        public IDictionary<string, string> FeatureValues { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public EditionfeatureCacheItem()
        {
            FeatureValues = new Dictionary<string, string>();
        }
    }
}