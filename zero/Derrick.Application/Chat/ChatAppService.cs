using System.Collections.Generic;
using System.Data.Entity;
using Abp.Domain.Repositories;
using Derrick.Chat.Dto;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using Abp.RealTime;
using Abp.Runtime.Session;
using Abp.Timing;
using Derrick.Friendships.Cache;
using Derrick.Friendships.Dto;

namespace Derrick.Chat
{
    /// <summary>
    /// 聊天服务实现
    /// </summary>
    public class ChatAppService : AbpZeroTemplateAppServiceBase, IChatAppService
    {
        /// <summary>
        /// 聊天消息仓储
        /// </summary>
        private readonly IRepository<ChatMessage, long> _chatMessageRepository;
        /// <summary>
        /// 用户好友缓存
        /// </summary>
        private readonly IUserFriendsCache _userFriendsCache;
        /// <summary>
        /// 在线客户端管理
        /// </summary>
        private readonly IOnlineClientManager _onlineClientManager;
        /// <summary>
        /// 聊天沟通器
        /// </summary>
        private readonly IChatCommunicator _chatCommunicator;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="chatMessageRepository">聊天消息仓储</param>
        /// <param name="userFriendsCache">用户好友缓存</param>
        /// <param name="onlineClientManager">在线客户端管理</param>
        /// <param name="chatCommunicator">聊天沟通器</param>
        public ChatAppService(
            IRepository<ChatMessage, long> chatMessageRepository,
            IUserFriendsCache userFriendsCache,
            IOnlineClientManager onlineClientManager,
            IChatCommunicator chatCommunicator)
        {
            _chatMessageRepository = chatMessageRepository;
            _userFriendsCache = userFriendsCache;
            _onlineClientManager = onlineClientManager;
            _chatCommunicator = chatCommunicator;
        }

        /// <summary>
        /// 获取用户聊天好友以及设置
        /// </summary>
        /// <returns></returns>
        [DisableAuditing]
        public GetUserChatFriendsWithSettingsOutput GetUserChatFriendsWithSettings()
        {
            var cacheItem = _userFriendsCache.GetCacheItem(AbpSession.ToUserIdentifier());

            var friends = cacheItem.Friends.MapTo<List<FriendDto>>();

            foreach (var friend in friends)
            {
                friend.IsOnline = _onlineClientManager.IsOnline(
                    new UserIdentifier(friend.FriendTenantId, friend.FriendUserId)
                );
            }

            return new GetUserChatFriendsWithSettingsOutput
            {
                Friends = friends,
                ServerTime = Clock.Now
            };
        }
        /// <summary>
        /// 获取用户聊天消息
        /// </summary>
        /// <param name="input">用户聊天消息Input</param>
        /// <returns></returns>
        [DisableAuditing]
        public async Task<ListResultDto<ChatMessageDto>> GetUserChatMessages(GetUserChatMessagesInput input)
        {
            var userId = AbpSession.GetUserId();
            var messages = await _chatMessageRepository.GetAll()
                    .WhereIf(input.MinMessageId.HasValue, m => m.Id < input.MinMessageId.Value)
                    .Where(m => m.UserId == userId && m.TargetTenantId == input.TenantId && m.TargetUserId == input.UserId)
                    .OrderByDescending(m => m.CreationTime)
                    .Take(50)
                    .ToListAsync();

            messages.Reverse();

            return new ListResultDto<ChatMessageDto>(messages.MapTo<List<ChatMessageDto>>());
        }
        /// <summary>
        /// 标记所有未读消息作为已读
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task MarkAllUnreadMessagesOfUserAsRead(MarkAllUnreadMessagesOfUserAsReadInput input)
        {
            var userId = AbpSession.GetUserId();
            var messages = await _chatMessageRepository
                .GetAll()
                .Where(m =>
                    m.UserId == userId &&
                    m.TargetTenantId == input.TenantId &&
                    m.TargetUserId == input.UserId &&
                    m.ReadState == ChatMessageReadState.Unread)
                .ToListAsync();

            if (!messages.Any())
            {
                return;
            }

            foreach (var message in messages)
            {
                message.ChangeReadState(ChatMessageReadState.Read);
            }

            var userIdentifier = AbpSession.ToUserIdentifier();
            var friendIdentifier = input.ToUserIdentifier();

            _userFriendsCache.ResetUnreadMessageCount(userIdentifier, friendIdentifier);

            var onlineClients = _onlineClientManager.GetAllByUserId(userIdentifier);
            if (onlineClients.Any())
            {
                _chatCommunicator.SendAllUnreadMessagesOfUserReadToClients(onlineClients, friendIdentifier);
            }
        }
    }
}