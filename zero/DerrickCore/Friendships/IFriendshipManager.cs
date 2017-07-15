using Abp;
using Abp.Domain.Services;

namespace Derrick.Friendships
{
    /// <summary>
    /// 好友管理器接口
    /// </summary>
    public interface IFriendshipManager : IDomainService
    {
        /// <summary>
        /// 创建好友
        /// </summary>
        /// <param name="friendship">好友实体</param>
        void CreateFriendship(Friendship friendship);
        /// <summary>
        /// 更新好友
        /// </summary>
        /// <param name="friendship">好友实体</param>
        void UpdateFriendship(Friendship friendship);
        /// <summary>
        /// 获取好友或Null
        /// </summary>
        /// <param name="user">用户标识</param>
        /// <param name="probableFriend">可能的好友</param>
        /// <returns></returns>
        Friendship GetFriendshipOrNull(UserIdentifier user, UserIdentifier probableFriend);
        /// <summary>
        /// 禁止的好友
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <param name="probableFriend">可能的好友</param>
        void BanFriend(UserIdentifier userIdentifier, UserIdentifier probableFriend);
        /// <summary>
        /// 接受好友请求
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <param name="probableFriend">可能的好友</param>
        void AcceptFriendshipRequest(UserIdentifier userIdentifier, UserIdentifier probableFriend);
    }
}
