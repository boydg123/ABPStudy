using Abp;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Derrick.Chat;

namespace Derrick.Friendships.Cache
{
    /// <summary>
    /// 用户好友缓存同步器
    /// </summary>
    public class UserFriendCacheSyncronizer :
        IEventHandler<EntityCreatedEventData<Friendship>>,
        IEventHandler<EntityDeletedEventData<Friendship>>,
        IEventHandler<EntityUpdatedEventData<Friendship>>,
        IEventHandler<EntityCreatedEventData<ChatMessage>>,
        ITransientDependency
    {
        /// <summary>
        /// 用户好友缓存接口
        /// </summary>
        private readonly IUserFriendsCache _userFriendsCache;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userFriendsCache">用户好友缓存接口</param>
        public UserFriendCacheSyncronizer(
            IUserFriendsCache userFriendsCache)
        {
            _userFriendsCache = userFriendsCache;
        }
        /// <summary>
        /// 处理好友创建事件
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void HandleEvent(EntityCreatedEventData<Friendship> eventData)
        {
            _userFriendsCache.AddFriend(
                eventData.Entity.ToUserIdentifier(),
                eventData.Entity.MapTo<FriendCacheItem>()
                );
        }
        /// <summary>
        /// 处理好友删除事件
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void HandleEvent(EntityDeletedEventData<Friendship> eventData)
        {
            _userFriendsCache.RemoveFriend(
                eventData.Entity.ToUserIdentifier(),
                eventData.Entity.MapTo<FriendCacheItem>()
            );
        }
        /// <summary>
        /// 处理好友更新事件
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void HandleEvent(EntityUpdatedEventData<Friendship> eventData)
        {
            var friendCacheItem = eventData.Entity.MapTo<FriendCacheItem>();
            _userFriendsCache.UpdateFriend(eventData.Entity.ToUserIdentifier(), friendCacheItem);
        }
        /// <summary>
        /// 处理聊天消息创建事件
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void HandleEvent(EntityCreatedEventData<ChatMessage> eventData)
        {
            var message = eventData.Entity;
            if (message.ReadState == ChatMessageReadState.Unread)
            {
                _userFriendsCache.IncreaseUnreadMessageCount(
                    new UserIdentifier(message.TenantId, message.UserId),
                    new UserIdentifier(message.TargetTenantId, message.TargetUserId),
                    1
                );
            }
        }
    }
}
