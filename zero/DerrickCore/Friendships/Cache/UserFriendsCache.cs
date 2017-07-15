using Abp;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Derrick.Chat;
using System.Linq;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Microsoft.AspNet.Identity;
using Derrick.Authorization.Users;

namespace Derrick.Friendships.Cache
{
    /// <summary>
    /// 用户好友缓存实现
    /// </summary>
    public class UserFriendsCache : IUserFriendsCache, ISingletonDependency
    {
        /// <summary>
        /// 缓存管理引用
        /// </summary>
        private readonly ICacheManager _cacheManager;
        /// <summary>
        /// 好友仓储
        /// </summary>
        private readonly IRepository<Friendship, long> _friendshipRepository;
        /// <summary>
        /// 聊天消息仓储
        /// </summary>
        private readonly IRepository<ChatMessage, long> _chatMessageRepository;
        /// <summary>
        /// 商户缓存引用
        /// </summary>
        private readonly ITenantCache _tenantCache;
        /// <summary>
        /// 用户管理引用
        /// </summary>
        private readonly UserManager _userManager;
        /// <summary>
        /// 工作单元引用
        /// </summary>
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly object _syncObj = new object();
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheManager">缓存管理引用</param>
        /// <param name="friendshipRepository">好友仓储</param>
        /// <param name="chatMessageRepository">聊天消息仓储</param>
        /// <param name="tenantCache">商户缓存引用</param>
        /// <param name="userManager">用户管理引用</param>
        /// <param name="unitOfWorkManager">工作单元引用</param>
        public UserFriendsCache(
            ICacheManager cacheManager,
            IRepository<Friendship, long> friendshipRepository,
            IRepository<ChatMessage, long> chatMessageRepository,
            ITenantCache tenantCache,
            UserManager userManager,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _cacheManager = cacheManager;
            _friendshipRepository = friendshipRepository;
            _chatMessageRepository = chatMessageRepository;
            _tenantCache = tenantCache;
            _userManager = userManager;
            _unitOfWorkManager = unitOfWorkManager;
        }

        /// <summary>
        /// 获取缓存项
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual UserWithFriendsCacheItem GetCacheItem(UserIdentifier userIdentifier)
        {
            return _cacheManager
                .GetCache(FriendCacheItem.CacheName)
                .Get<string, UserWithFriendsCacheItem>(userIdentifier.ToUserIdentifierString(), f => GetUserFriendsCacheItemInternal(userIdentifier));
        }
        /// <summary>
        /// 获取缓存项或Null
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <returns></returns>
        public virtual UserWithFriendsCacheItem GetCacheItemOrNull(UserIdentifier userIdentifier)
        {
            return _cacheManager
                .GetCache(FriendCacheItem.CacheName)
                .GetOrDefault<string, UserWithFriendsCacheItem>(userIdentifier.ToUserIdentifierString());
        }
        /// <summary>
        /// 重置未读消息数量
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <param name="friendIdentifier">用户好友标识</param>
        [UnitOfWork]
        public virtual void ResetUnreadMessageCount(UserIdentifier userIdentifier, UserIdentifier friendIdentifier)
        {
            var user = GetCacheItemOrNull(userIdentifier);
            if (user == null)
            {
                return;
            }

            lock (_syncObj)
            {
                var friend = user.Friends.FirstOrDefault(
                    f => f.FriendUserId == friendIdentifier.UserId &&
                         f.FriendTenantId == friendIdentifier.TenantId
                );

                if (friend == null)
                {
                    return;
                }

                friend.UnreadMessageCount = 0;
            }
        }

        /// <summary>
        /// 增加未读消息数量
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <param name="friendIdentifier">用户好友标识</param>
        /// <param name="change">修改的数量</param>
        [UnitOfWork]
        public virtual void IncreaseUnreadMessageCount(UserIdentifier userIdentifier, UserIdentifier friendIdentifier, int change)
        {
            var user = GetCacheItemOrNull(userIdentifier);
            if (user == null)
            {
                return;
            }

            lock (_syncObj)
            {
                var friend = user.Friends.FirstOrDefault(
                    f => f.FriendUserId == friendIdentifier.UserId &&
                         f.FriendTenantId == friendIdentifier.TenantId
                );

                if (friend == null)
                {
                    return;
                }

                friend.UnreadMessageCount += change;
            }
        }
        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <param name="friend">好友缓存项</param>
        public void AddFriend(UserIdentifier userIdentifier, FriendCacheItem friend)
        {
            var user = GetCacheItemOrNull(userIdentifier);
            if (user == null)
            {
                return;
            }

            lock (_syncObj)
            {
                if (!user.Friends.ContainsFriend(friend))
                {
                    user.Friends.Add(friend);
                }
            }
        }
        /// <summary>
        /// 移除好友
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <param name="friend">好友缓存项</param>
        public void RemoveFriend(UserIdentifier userIdentifier, FriendCacheItem friend)
        {
            var user = GetCacheItemOrNull(userIdentifier);
            if (user == null)
            {
                return;
            }

            lock (_syncObj)
            {
                if (user.Friends.ContainsFriend(friend))
                {
                    user.Friends.Remove(friend);
                }
            }
        }
        /// <summary>
        /// 更新好友
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <param name="friend">好友缓存项</param>
        public void UpdateFriend(UserIdentifier userIdentifier, FriendCacheItem friend)
        {
            var user = GetCacheItemOrNull(userIdentifier);
            if (user == null)
            {
                return;
            }

            lock (_syncObj)
            {
                var existingFriendIndex = user.Friends.FindIndex(
                    f => f.FriendUserId == friend.FriendUserId &&
                         f.FriendTenantId == friend.FriendTenantId
                );

                if (existingFriendIndex >= 0)
                {
                    user.Friends[existingFriendIndex] = friend;
                }
            }
        }
        /// <summary>
        /// 内部获取用户好友缓存项
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <returns></returns>
        [UnitOfWork]
        protected virtual UserWithFriendsCacheItem GetUserFriendsCacheItemInternal(UserIdentifier userIdentifier)
        {
            var tenancyName = userIdentifier.TenantId.HasValue
                ? _tenantCache.GetOrNull(userIdentifier.TenantId.Value)?.TenancyName
                : null;

            using (_unitOfWorkManager.Current.SetTenantId(userIdentifier.TenantId))
            {
                var friendCacheItems =
                    (from friendship in _friendshipRepository.GetAll()
                     join chatMessage in _chatMessageRepository.GetAll() on
                     new { UserId = userIdentifier.UserId, TenantId = userIdentifier.TenantId, TargetUserId = friendship.FriendUserId, TargetTenantId = friendship.FriendTenantId, ChatSide = ChatSide.Receiver } equals
                     new { UserId = chatMessage.UserId, TenantId = chatMessage.TenantId, TargetUserId = chatMessage.TargetUserId, TargetTenantId = chatMessage.TargetTenantId, ChatSide = chatMessage.Side } into chatMessageJoined
                     where friendship.UserId == userIdentifier.UserId
                     select new FriendCacheItem
                     {
                         FriendUserId = friendship.FriendUserId,
                         FriendTenantId = friendship.FriendTenantId,
                         State = friendship.State,
                         FriendUserName = friendship.FriendUserName,
                         FriendTenancyName = friendship.FriendTenancyName,
                         FriendProfilePictureId = friendship.FriendProfilePictureId,
                         UnreadMessageCount = chatMessageJoined.Count(cm => cm.ReadState == ChatMessageReadState.Unread)
                     }).ToList();

                var user = _userManager.FindById(userIdentifier.UserId);

                return new UserWithFriendsCacheItem
                {
                    TenantId = userIdentifier.TenantId,
                    UserId = userIdentifier.UserId,
                    TenancyName = tenancyName,
                    UserName = user.UserName,
                    ProfilePictureId = user.ProfilePictureId,
                    Friends = friendCacheItems
                };
            }
        }
    }
}