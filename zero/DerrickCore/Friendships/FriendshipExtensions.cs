using Abp;

namespace Derrick.Friendships
{
    /// <summary>
    /// 友情扩展
    /// </summary>
    public static class FriendshipExtensions
    {
        /// <summary>
        /// 将好友实体转换成用户标识
        /// </summary>
        /// <param name="friendship">好友实体</param>
        /// <returns></returns>
        public static UserIdentifier ToUserIdentifier(this Friendship friendship)
        {
            return new UserIdentifier(friendship.TenantId, friendship.UserId);
        }

        /// <summary>
        /// 将好友实体转换成好友用户标识
        /// </summary>
        /// <param name="friendship">好友实体</param>
        /// <returns></returns>
        public static UserIdentifier ToFriendIdentifier(this Friendship friendship)
        {
            return new UserIdentifier(friendship.FriendTenantId, friendship.FriendUserId);
        }
    }
}
