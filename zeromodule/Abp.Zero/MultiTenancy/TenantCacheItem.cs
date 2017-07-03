using System;

namespace Abp.MultiTenancy
{
    /// <summary>
    /// 商户缓存项
    /// </summary>
    [Serializable]
    public class TenantCacheItem
    {
        /// <summary>
        /// 缓存名称
        /// </summary>
        public const string CacheName = "AbpZeroTenantCache";
        /// <summary>
        /// 商户缓存名的名称
        /// </summary>
        public const string ByNameCacheName = "AbpZeroTenantByNameCache";
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 商户名称
        /// </summary>
        public string TenancyName { get; set; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public int? EditionId { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 自定义数据
        /// </summary>
        public object CustomData { get; set; }
    }
}