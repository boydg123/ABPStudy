using System.Collections.Generic;

namespace Derrick.Notifications.Dto
{
    /// <summary>
    /// 获取通知设置Output
    /// </summary>
    public class GetNotificationSettingsOutput
    {
        /// <summary>
        /// 是否收到通知
        /// </summary>
        public bool ReceiveNotifications { get; set; }
        /// <summary>
        /// 通知列表
        /// </summary>
        public List<NotificationSubscriptionWithDisplayNameDto> Notifications { get; set; }
    }
}