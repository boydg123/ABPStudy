using Abp;

namespace Derrick.Friendships.Cache
{
    /// <summary>
    /// 用户好友缓存接口
    /// </summary>
    public interface IUserFriendsCache
    {
        /// <summary>
        /// 获取用户好友缓存项
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <returns></returns>
        UserWithFriendsCacheItem GetCacheItem(UserIdentifier userIdentifier);

        /// <summary>
        /// 获取用户好友缓存项或Null
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <returns></returns>
        UserWithFriendsCacheItem GetCacheItemOrNull(UserIdentifier userIdentifier);
        /// <summary>
        /// 重置未读消息数量
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <param name="friendIdentifier">用户好友标识</param>
        void ResetUnreadMessageCount(UserIdentifier userIdentifier, UserIdentifier friendIdentifier);
        /// <summary>
        /// 增加未读消息数量
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <param name="friendIdentifier">用户好友标识</param>
        /// <param name="change">修改的数量</param>
        void IncreaseUnreadMessageCount(UserIdentifier userIdentifier, UserIdentifier friendIdentifier, int change);
        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <param name="friend">好友缓存项</param>
        void AddFriend(UserIdentifier userIdentifier, FriendCacheItem friend);
        /// <summary>
        /// 移除好友
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <param name="friend">好友缓存项</param>
        void RemoveFriend(UserIdentifier userIdentifier, FriendCacheItem friend);
        /// <summary>
        /// 更新好友
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <param name="friend">好友缓存项</param>
        void UpdateFriend(UserIdentifier userIdentifier, FriendCacheItem friend);
    }
}
