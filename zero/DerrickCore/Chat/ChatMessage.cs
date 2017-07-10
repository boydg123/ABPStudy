using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;

namespace Derrick.Chat
{
    /// <summary>
    /// 聊天消息
    /// </summary>
    [Table("AppChatMessages")]
    public class ChatMessage : Entity<long>, IHasCreationTime, IMayHaveTenant
    {
        /// <summary>
        /// 最大消息长度
        /// </summary>
        public const int MaxMessageLength = 4 * 1024; //4KB
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
        /// 消息
        /// </summary>
        [Required]
        [StringLength(MaxMessageLength)]
        public string Message { get; set; }
        /// <summary>
        /// 创建事件
        /// </summary>
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// 聊天边
        /// </summary>
        public ChatSide Side { get; set; }
        /// <summary>
        /// 聊天消息读取状态
        /// </summary>
        public ChatMessageReadState ReadState { get; private set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="user">用户标识</param>
        /// <param name="targetUser">目标用户标识</param>
        /// <param name="side">聊天边</param>
        /// <param name="message">消息</param>
        /// <param name="readState">消息读取状态</param>
        public ChatMessage(
            UserIdentifier user,
            UserIdentifier targetUser,
            ChatSide side,
            string message,
            ChatMessageReadState readState)
        {
            UserId = user.UserId;
            TenantId = user.TenantId;
            TargetUserId = targetUser.UserId;
            TargetTenantId = targetUser.TenantId;
            Message = message;
            Side = side;
            ReadState = readState;

            CreationTime = Clock.Now;
        }
        /// <summary>
        /// 修改读取状态
        /// </summary>
        /// <param name="newState"></param>
        public void ChangeReadState(ChatMessageReadState newState)
        {
            ReadState = newState;
        }

        protected ChatMessage()
        {

        }
    }
}
