using System;
using Abp.Application.Services.Dto;

namespace Abp.Notifications
{
    /// <summary>
    /// Represents a notification sent to a user.
    /// 表示发送给用户的通知
    /// </summary>
    [Serializable]
    public class UserNotification : EntityDto<Guid>, IUserIdentifier
    {
        /// <summary>
        /// TenantId.
        /// 租户ID
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// User Id.
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Current state of the user notification.
        /// 用户通知的当前状态
        /// </summary>
        public UserNotificationState State { get; set; }

        /// <summary>
        /// The notification.
        /// 通知
        /// </summary>
        public TenantNotification Notification { get; set; }
    }
}