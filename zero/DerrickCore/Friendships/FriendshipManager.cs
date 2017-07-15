using System;
using Abp;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;

namespace Derrick.Friendships
{
    /// <summary>
    /// <see cref="IFriendshipManager"/>实现。好友管理实现
    /// </summary>
    public class FriendshipManager : AbpZeroTemplateDomainServiceBase, IFriendshipManager
    {
        /// <summary>
        /// 好友仓储
        /// </summary>
        private readonly IRepository<Friendship, long> _friendshipRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="friendshipRepository">好友仓储</param>
        public FriendshipManager(IRepository<Friendship, long> friendshipRepository)
        {
            _friendshipRepository = friendshipRepository;
        }
        /// <summary>
        /// 创建好友
        /// </summary>
        /// <param name="friendship">好友实体</param>
        [UnitOfWork]
        public void CreateFriendship(Friendship friendship)
        {
            if (friendship.TenantId == friendship.FriendTenantId &&
                friendship.UserId == friendship.FriendUserId)
            {
                throw new UserFriendlyException(L("YouCannotBeFriendWithYourself"));
            }

            using (CurrentUnitOfWork.SetTenantId(friendship.TenantId))
            {
                _friendshipRepository.Insert(friendship);
                CurrentUnitOfWork.SaveChanges();
            }
        }
        /// <summary>
        /// 更新好友
        /// </summary>
        /// <param name="friendship">好友实体</param>
        [UnitOfWork]
        public void UpdateFriendship(Friendship friendship)
        {
            using (CurrentUnitOfWork.SetTenantId(friendship.TenantId))
            {
                _friendshipRepository.Update(friendship);
                CurrentUnitOfWork.SaveChanges();
            }
        }
        /// <summary>
        /// 获取好友或Null
        /// </summary>
        /// <param name="user">用户标识</param>
        /// <param name="probableFriend">可能的好友</param>
        /// <returns></returns>
        [UnitOfWork]
        public Friendship GetFriendshipOrNull(UserIdentifier user, UserIdentifier probableFriend)
        {
            using (CurrentUnitOfWork.SetTenantId(user.TenantId))
            {
                return _friendshipRepository.FirstOrDefault(friendship =>
                                    friendship.UserId == user.UserId &&
                                    friendship.TenantId == user.TenantId &&
                                    friendship.FriendUserId == probableFriend.UserId &&
                                    friendship.FriendTenantId == probableFriend.TenantId);
            }
        }
        /// <summary>
        /// 禁止的好友
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <param name="probableFriend">可能的好友</param>
        [UnitOfWork]
        public void BanFriend(UserIdentifier userIdentifier, UserIdentifier probableFriend)
        {
            var friendship = GetFriendshipOrNull(userIdentifier, probableFriend);
            if (friendship == null)
            {
                throw new ApplicationException("Friendship does not exist between " + userIdentifier + " and " + probableFriend);
            }

            friendship.State = FriendshipState.Blocked;
            UpdateFriendship(friendship);
        }
        /// <summary>
        /// 接受好友请求
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <param name="probableFriend">可能的好友</param>
        [UnitOfWork]
        public void AcceptFriendshipRequest(UserIdentifier userIdentifier, UserIdentifier probableFriend)
        {
            var friendship = GetFriendshipOrNull(userIdentifier, probableFriend);
            if (friendship == null)
            {
                throw new ApplicationException("Friendship does not exist between " + userIdentifier + " and " + probableFriend);
            }

            friendship.State = FriendshipState.Accepted;
            UpdateFriendship(friendship);
        }
    }
}