using Abp;

namespace Derrick.Chat.Dto
{
    /// <summary>
    /// 标记所有未读消息为已读Input
    /// </summary>
    public class MarkAllUnreadMessagesOfUserAsReadInput
    {
        /// <summary>
        /// 商户ID
        /// </summary>
        public int? TenantId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 转换成用户
        /// </summary>
        /// <returns></returns>
        public UserIdentifier ToUserIdentifier()
        {
            return new UserIdentifier(TenantId, UserId);
        }
    }
}
