using System.Collections.Generic;

namespace Derrick.Chat.Dto
{
    /// <summary>
    /// 聊天用户以及消息Dto
    /// </summary>
    public class ChatUserWithMessagesDto : ChatUserDto
    {
        /// <summary>
        /// 消息列表
        /// </summary>
        public List<ChatMessageDto> Messages { get; set; }
    }
}