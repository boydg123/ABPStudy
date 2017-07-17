using System.Collections.Generic;

namespace Derrick.Notifications.Dto
{
    /// <summary>
    /// 更新通知设置Input
    /// </summary>
    public class UpdateNotificationSettingsInput
    {
        /// <summary>
        /// 是否收到通知
        /// </summary>
        public bool ReceiveNotifications { get; set; }
        /// <summary>
        /// 订阅通知列表
        /// </summary>
        public List<NotificationSubscriptionDto> Notifications { get; set; }
    }
}