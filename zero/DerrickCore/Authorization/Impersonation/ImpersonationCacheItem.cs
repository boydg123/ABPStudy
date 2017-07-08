using System;

namespace Derrick.Authorization.Impersonation
{
    /// <summary>
    /// 模拟缓存项
    /// </summary>
    [Serializable]
    public class ImpersonationCacheItem
    {
        /// <summary>
        /// 缓存名称
        /// </summary>
        public const string CacheName = "AppImpersonationCache";
        /// <summary>
        /// 模拟商户ID
        /// </summary>
        public int? ImpersonatorTenantId { get; set; }
        /// <summary>
        /// 模拟用户ID
        /// </summary>
        public long ImpersonatorUserId { get; set; }
        /// <summary>
        /// 目标商户ID
        /// </summary>
        public int? TargetTenantId { get; set; }
        /// <summary>
        /// 目标用户ID
        /// </summary>
        public long TargetUserId { get; set; }
        /// <summary>
        /// 是否返回模拟者
        /// </summary>
        public bool IsBackToImpersonator { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public ImpersonationCacheItem()
        {
            
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="targetTenantId">目标商户ID</param>
        /// <param name="targetUserId">目标商户ID</param>
        /// <param name="isBackToImpersonator">是否返回模拟者</param>
        public ImpersonationCacheItem(int? targetTenantId, long targetUserId, bool isBackToImpersonator)
        {
            TargetTenantId = targetTenantId;
            TargetUserId = targetUserId;
            IsBackToImpersonator = isBackToImpersonator;
        }
    }
}