using System;
using System.Linq;
using Abp;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.RealTime;
using Abp.UI;
using Derrick.Authorization.Users;
using Derrick.Friendships;
using Derrick.Friendships.Cache;

namespace Derrick.Chat
{
    /// <summary>
    /// 聊天消息管理器实现
    /// </summary>
    [AbpAuthorize]
    public class ChatMessageManager : AbpZeroTemplateDomainServiceBase, IChatMessageManager
    {
        /// <summary>
        /// 友谊管理引用
        /// </summary>
        private readonly IFriendshipManager _friendshipManager;
        /// <summary>
        /// 聊天沟通引用
        /// </summary>
        private readonly IChatCommunicator _chatCommunicator;
        /// <summary>
        /// 在线客户管理引用
        /// </summary>
        private readonly IOnlineClientManager _onlineClientManager;
        /// <summary>
        /// 用户管理引用
        /// </summary>
        private readonly UserManager _userManager;
        /// <summary>
        /// 商户缓存引用
        /// </summary>
        private readonly ITenantCache _tenantCache;
        /// <summary>
        /// 友谊缓存引用
        /// </summary>
        private readonly IUserFriendsCache _userFriendsCache;
        /// <summary>
        /// 用户邮件发送引用
        /// </summary>
        private readonly IUserEmailer _userEmailer;
        /// <summary>
        /// 聊天消息仓储
        /// </summary>
        private readonly IRepository<ChatMessage, long> _chatMessageRepository;
        /// <summary>
        /// 聊天功能检查器引用
        /// </summary>
        private readonly IChatFeatureChecker _chatFeatureChecker;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="friendshipManager">友谊管理引用</param>
        /// <param name="chatCommunicator">聊天沟通引用</param>
        /// <param name="onlineClientManager">在线客户管理引用</param>
        /// <param name="userManager">用户管理引用</param>
        /// <param name="tenantCache">商户缓存引用</param>
        /// <param name="userFriendsCache">友谊缓存引用</param>
        /// <param name="userEmailer">用户邮件发送引用</param>
        /// <param name="chatMessageRepository">聊天消息仓储</param>
        /// <param name="chatFeatureChecker">聊天功能检查器引用</param>
        public ChatMessageManager(
            IFriendshipManager friendshipManager,
            IChatCommunicator chatCommunicator,
            IOnlineClientManager onlineClientManager,
            UserManager userManager,
            ITenantCache tenantCache,
            IUserFriendsCache userFriendsCache,
            IUserEmailer userEmailer,
            IRepository<ChatMessage, long> chatMessageRepository,
            IChatFeatureChecker chatFeatureChecker)
        {
            _friendshipManager = friendshipManager;
            _chatCommunicator = chatCommunicator;
            _onlineClientManager = onlineClientManager;
            _userManager = userManager;
            _tenantCache = tenantCache;
            _userFriendsCache = userFriendsCache;
            _userEmailer = userEmailer;
            _chatMessageRepository = chatMessageRepository;
            _chatFeatureChecker = chatFeatureChecker;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sender">发送人标识</param>
        /// <param name="receiver">接收人标识</param>
        /// <param name="message">消息</param>
        /// <param name="senderTenancyName">发送者商户名称</param>
        /// <param name="senderUserName">发送者用户名</param>
        /// <param name="senderProfilePictureId">发送人图片</param>
        public void SendMessage(UserIdentifier sender, UserIdentifier receiver, string message, string senderTenancyName, string senderUserName, Guid? senderProfilePictureId)
        {
            CheckReceiverExists(receiver);

            _chatFeatureChecker.CheckChatFeatures(sender.TenantId, receiver.TenantId);
            
            var friendshipState = _friendshipManager.GetFriendshipOrNull(sender, receiver)?.State;
            if (friendshipState == FriendshipState.Blocked)
            {
                throw new UserFriendlyException(L("UserIsBlocked"));
            }
            
            HandleSenderToReceiver(sender, receiver, message);
            HandleReceiverToSender(sender, receiver, message);
            HandleSenderUserInfoChange(sender, receiver, senderTenancyName, senderUserName, senderProfilePictureId);
        }

        /// <summary>
        /// 检测接受人是否存在
        /// </summary>
        /// <param name="receiver"></param>
        private void CheckReceiverExists(UserIdentifier receiver)
        {
            var receiverUser = _userManager.GetUserOrNull(receiver);
            if (receiverUser == null)
            {
                throw new UserFriendlyException(L("TargetUserNotFoundProbablyDeleted"));
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="message">聊天消息</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual long Save(ChatMessage message)
        {
            using (CurrentUnitOfWork.SetTenantId(message.TenantId))
            {
                return _chatMessageRepository.InsertAndGetId(message);
            }
        }

        /// <summary>
        /// 获取未读消息数量
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <param name="sender">发送者标识</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual int GetUnreadMessageCount(UserIdentifier sender, UserIdentifier receiver)
        {
            using (CurrentUnitOfWork.SetTenantId(receiver.TenantId))
            {
                return _chatMessageRepository.Count(cm => cm.UserId == receiver.UserId && cm.TargetUserId == sender.UserId && cm.TargetTenantId == sender.TenantId && cm.ReadState == ChatMessageReadState.Unread);
            }
        }

        /// <summary>
        /// 处理消息发送到接收
        /// </summary>
        /// <param name="senderIdentifier">发送者标识</param>
        /// <param name="receiverIdentifier">接收者标识</param>
        /// <param name="message">消息</param>
        private void HandleSenderToReceiver(UserIdentifier senderIdentifier, UserIdentifier receiverIdentifier, string message)
        {
            var friendshipState = _friendshipManager.GetFriendshipOrNull(senderIdentifier, receiverIdentifier)?.State;
            if (friendshipState == null)
            {
                friendshipState = FriendshipState.Accepted;

                var receiverTenancyName = receiverIdentifier.TenantId.HasValue
                    ? _tenantCache.Get(receiverIdentifier.TenantId.Value).TenancyName
                    : null;

                var receiverUser = _userManager.GetUser(receiverIdentifier);
                _friendshipManager.CreateFriendship(
                    new Friendship(
                        senderIdentifier,
                        receiverIdentifier,
                        receiverTenancyName,
                        receiverUser.UserName,
                        receiverUser.ProfilePictureId,
                        friendshipState.Value)
                );
            }

            if (friendshipState.Value == FriendshipState.Blocked)
            {
                //Do not send message if receiver banned the sender
                return;
            }

            var sentMessage = new ChatMessage(
                senderIdentifier,
                receiverIdentifier,
                ChatSide.Sender,
                message,
                ChatMessageReadState.Read
            );

            Save(sentMessage);

            _chatCommunicator.SendMessageToClient(
                _onlineClientManager.GetAllByUserId(senderIdentifier),
                sentMessage
                );
        }

        /// <summary>
        /// 处理接收消息到发送者
        /// </summary>
        /// <param name="senderIdentifier">发送者标识</param>
        /// <param name="receiverIdentifier">接收者标识</param>
        /// <param name="message">消息</param>
        private void HandleReceiverToSender(UserIdentifier senderIdentifier, UserIdentifier receiverIdentifier, string message)
        {
            var friendshipState = _friendshipManager.GetFriendshipOrNull(receiverIdentifier, senderIdentifier)?.State;

            if (friendshipState == null)
            {
                var senderTenancyName = senderIdentifier.TenantId.HasValue ?
                    _tenantCache.Get(senderIdentifier.TenantId.Value).TenancyName :
                    null;

                var senderUser = _userManager.GetUser(senderIdentifier);
                _friendshipManager.CreateFriendship(
                    new Friendship(
                        receiverIdentifier,
                        senderIdentifier,
                        senderTenancyName,
                        senderUser.UserName,
                        senderUser.ProfilePictureId,
                        FriendshipState.Accepted
                    )
                );
            }

            if (friendshipState == FriendshipState.Blocked)
            {
                //Do not send message if receiver banned the sender
                return;
            }

            var sentMessage = new ChatMessage(
                    receiverIdentifier,
                    senderIdentifier,
                    ChatSide.Receiver,
                    message,
                    ChatMessageReadState.Unread);

            Save(sentMessage);

            var clients = _onlineClientManager.GetAllByUserId(receiverIdentifier);
            if (clients.Any())
            {
                _chatCommunicator.SendMessageToClient(clients, sentMessage);
            }
            else if (GetUnreadMessageCount(senderIdentifier, receiverIdentifier) == 1)
            {
                var senderTenancyName = senderIdentifier.TenantId.HasValue ?
                    _tenantCache.Get(senderIdentifier.TenantId.Value).TenancyName :
                    null;

                _userEmailer.TryToSendChatMessageMail(
                    _userManager.GetUser(receiverIdentifier),
                    _userManager.GetUser(senderIdentifier).UserName,
                    senderTenancyName,
                    sentMessage
                );
            }
        }
        /// <summary>
        /// 处理发送用户信息修改
        /// </summary>
        /// <param name="sender">发送者标识</param>
        /// <param name="receiver">接受者标识</param>
        /// <param name="senderTenancyName">发送者商户名</param>
        /// <param name="senderUserName">发送者用户名</param>
        /// <param name="senderProfilePictureId">发送者图片ID</param>
        private void HandleSenderUserInfoChange(UserIdentifier sender, UserIdentifier receiver, string senderTenancyName, string senderUserName, Guid? senderProfilePictureId)
        {
            var receiverCacheItem = _userFriendsCache.GetCacheItemOrNull(receiver);
            if (receiverCacheItem == null)
            {
                return;
            }

            var senderAsFriend = receiverCacheItem.Friends.FirstOrDefault(f => f.FriendTenantId == sender.TenantId && f.FriendUserId == sender.UserId);
            if (senderAsFriend == null)
            {
                return;
            }

            if (senderAsFriend.FriendTenancyName == senderTenancyName &&
                senderAsFriend.FriendUserName == senderUserName &&
                senderAsFriend.FriendProfilePictureId == senderProfilePictureId)
            {
                return;
            }

            var friendship = _friendshipManager.GetFriendshipOrNull(receiver, sender);
            if (friendship == null)
            {
                return;
            }

            friendship.FriendTenancyName = senderTenancyName;
            friendship.FriendUserName = senderUserName;
            friendship.FriendProfilePictureId = senderProfilePictureId;

            _friendshipManager.UpdateFriendship(friendship);
        }
    }
}