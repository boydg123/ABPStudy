using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Derrick.Chat.Dto;

namespace Derrick.Chat
{
    /// <summary>
    /// 聊天服务
    /// </summary>
    public interface IChatAppService : IApplicationService
    {
        /// <summary>
        /// 获取用户聊天好友以及设置
        /// </summary>
        /// <returns></returns>
        GetUserChatFriendsWithSettingsOutput GetUserChatFriendsWithSettings();
        /// <summary>
        /// 获取用户聊天消息
        /// </summary>
        /// <param name="input">用户聊天消息Input</param>
        /// <returns></returns>
        Task<ListResultDto<ChatMessageDto>> GetUserChatMessages(GetUserChatMessagesInput input);
        /// <summary>
        /// 标记所有未读消息作为已读
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task MarkAllUnreadMessagesOfUserAsRead(MarkAllUnreadMessagesOfUserAsReadInput input);
    }
}
