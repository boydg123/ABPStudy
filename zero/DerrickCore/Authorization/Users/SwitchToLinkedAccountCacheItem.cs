using System;

namespace Derrick.Authorization.Users
{
    /// <summary>
    /// 切换到连接用户的缓存项
    /// </summary>
    [Serializable]
    public class SwitchToLinkedAccountCacheItem
    {
        /// <summary>
        /// 缓存名称
        /// </summary>
        public const string CacheName = "AppSwitchToLinkedAccountCache";
        /// <summary>
        /// 目标商户ID
        /// </summary>
        public int? TargetTenantId { get; set; }
        /// <summary>
        /// 目标用户ID
        /// </summary>
        public long TargetUserId { get; set; }
        /// <summary>
        /// 模拟商户ID
        /// </summary>
        public int? ImpersonatorTenantId { get; set; }
        /// <summary>
        /// 模拟用户ID
        /// </summary>
        public long? ImpersonatorUserId { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public SwitchToLinkedAccountCacheItem()
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="targetTenantId">目标商户ID</param>
        /// <param name="targetUserId">目标用户ID</param>
        /// <param name="impersonatorTenantId">模拟商户ID</param>
        /// <param name="impersonatorUserId">模拟用户ID</param>
        public SwitchToLinkedAccountCacheItem(
            int? targetTenantId,
            long targetUserId,
            int? impersonatorTenantId,
            long? impersonatorUserId
            )
        {
            TargetTenantId = targetTenantId;
            TargetUserId = targetUserId;
            ImpersonatorTenantId = impersonatorTenantId;
            ImpersonatorUserId = impersonatorUserId;
        }
    }
}
