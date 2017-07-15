using System.Linq;
using Abp;
using Abp.Dependency;
using Abp.RealTime;
using Derrick.Chat;
using Derrick.Friendships.Cache;

namespace Derrick.Friendships
{
    /// <summary>
    /// 聊天用户状态观察器
    /// </summary>
    public class ChatUserStateWatcher : ISingletonDependency
    {
        /// <summary>
        /// 聊天沟通接口
        /// </summary>
        private readonly IChatCommunicator _chatCommunicator;
        /// <summary>
        /// 用户好友缓存引用
        /// </summary>
        private readonly IUserFriendsCache _userFriendsCache;
        /// <summary>
        /// 在线客户端管理器引用
        /// </summary>
        private readonly IOnlineClientManager _onlineClientManager;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="chatCommunicator">聊天沟通接口</param>
        /// <param name="userFriendsCache">用户好友缓存引用</param>
        /// <param name="onlineClientManager">在线客户端管理器引用</param>
        public ChatUserStateWatcher(
            IChatCommunicator chatCommunicator,
            IUserFriendsCache userFriendsCache,
            IOnlineClientManager onlineClientManager)
        {
            _chatCommunicator = chatCommunicator;
            _userFriendsCache = userFriendsCache;
            _onlineClientManager = onlineClientManager;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            _onlineClientManager.UserConnected += OnlineClientManager_UserConnected;
            _onlineClientManager.UserDisconnected += OnlineClientManager_UserDisconnected;
        }
        /// <summary>
        /// 在线客户端管理用户连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnlineClientManager_UserConnected(object sender, OnlineUserEventArgs e)
        {
            NotifyUserConnectionStateChange(e.User, true);
        }
        /// <summary>
        /// 在线客户端管理用户断开连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnlineClientManager_UserDisconnected(object sender, OnlineUserEventArgs e)
        {
            NotifyUserConnectionStateChange(e.User, false);
        }
        /// <summary>
        /// 通知用户连接状态修改
        /// </summary>
        /// <param name="user">用户标识</param>
        /// <param name="isConnected">是否连接</param>
        private void NotifyUserConnectionStateChange(UserIdentifier user, bool isConnected)
        {
            var cacheItem = _userFriendsCache.GetCacheItem(user);
           
            foreach (var friend in cacheItem.Friends)
            {
                var friendUserClients = _onlineClientManager.GetAllByUserId(new UserIdentifier(friend.FriendTenantId, friend.FriendUserId));
                if (!friendUserClients.Any())
                {
                    continue;
                }

                _chatCommunicator.SendUserConnectionChangeToClients(friendUserClients, user, isConnected);
            }
        }
    }
}