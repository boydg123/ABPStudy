using System;
using System.Collections.Generic;

namespace Derrick.Friendships.Cache
{
    /// <summary>
    /// 用户以及好友缓存项
    /// </summary>
    public class UserWithFriendsCacheItem
    {
        /// <summary>
        /// 商户ID
        /// </summary>
        public int? TenantId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 商户名称
        /// </summary>
        public string TenancyName { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 照片ID
        /// </summary>
        public Guid? ProfilePictureId { get; set; }
        /// <summary>
        /// 好友缓存项列表
        /// </summary>
        public List<FriendCacheItem> Friends { get; set; }
    }
}