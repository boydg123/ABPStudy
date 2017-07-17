using System.ComponentModel.DataAnnotations;

namespace Derrick.Chat.Dto
{
    /// <summary>
    /// 用户聊天消息Input
    /// </summary>
    public class GetUserChatMessagesInput
    {
        /// <summary>
        /// 商户ID
        /// </summary>
        public int? TenantId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }
        /// <summary>
        /// 最小消息ID
        /// </summary>
        public long? MinMessageId { get; set; }
    }
}