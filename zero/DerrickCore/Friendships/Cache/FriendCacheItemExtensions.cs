using System.Collections.Generic;
using System.Linq;

namespace Derrick.Friendships.Cache
{
    /// <summary>
    /// 好友缓存项扩展
    /// </summary>
    public static class FriendCacheItemExtensions
    {
        /// <summary>
        /// 是否包含好友
        /// </summary>
        /// <param name="items">好友缓存项列表</param>
        /// <param name="item">好友缓存项</param>
        /// <returns></returns>
        public static bool ContainsFriend(this List<FriendCacheItem> items, FriendCacheItem item)
        {
            return items.Any(f => f.FriendTenantId == item.FriendTenantId && f.FriendUserId == item.FriendUserId);
        }
    }
}
