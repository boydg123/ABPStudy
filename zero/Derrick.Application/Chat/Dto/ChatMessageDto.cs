using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Derrick.Chat.Dto
{
    /// <summary>
    /// 聊天消息Dto
    /// </summary>
    [AutoMapFrom(typeof(ChatMessage))]
    public class ChatMessageDto : EntityDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 商户ID
        /// </summary>
        public int? TenantId { get; set; }
        /// <summary>
        /// 目标用户ID
        /// </summary>
        public long TargetUserId { get; set; }
        /// <summary>
        /// 目标商户ID
        /// </summary>
        public int? TargetTenantId { get; set; }
        /// <summary>
        /// 聊天一边
        /// </summary>
        public ChatSide Side { get; set; }
        /// <summary>
        /// 聊天消息读取状态
        /// </summary>
        public ChatMessageReadState ReadState { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

    }
}