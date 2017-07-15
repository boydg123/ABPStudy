using System;
using Abp.AutoMapper;

namespace Derrick.Friendships.Cache
{
    /// <summary>
    /// 好友缓存项
    /// </summary>
    [AutoMapFrom(typeof(Friendship))]
    public class FriendCacheItem
    {
        /// <summary>
        /// 缓存名称
        /// </summary>
        public const string CacheName = "AppUserFriendCache";
        /// <summary>
        /// 好友用户ID
        /// </summary>
        public long FriendUserId { get; set; }
        /// <summary>
        /// 好友商户ID
        /// </summary>
        public int? FriendTenantId { get; set; }
        /// <summary>
        /// 好友用户名
        /// </summary>
        public string FriendUserName { get; set; }
        /// <summary>
        /// 好友商户名
        /// </summary>
        public string FriendTenancyName { get; set; }
        /// <summary>
        /// 好友照片ID
        /// </summary>
        public Guid? FriendProfilePictureId { get; set; }
        /// <summary>
        /// 未读消息数量
        /// </summary>
        public int UnreadMessageCount { get; set; }
        /// <summary>
        /// 好友状态
        /// </summary>
        public FriendshipState State { get; set; }
    }
}