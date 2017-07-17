using System.ComponentModel.DataAnnotations;
using Abp.Notifications;

namespace Derrick.Notifications.Dto
{
    /// <summary>
    /// 订阅通知Dto
    /// </summary>
    public class NotificationSubscriptionDto
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        [MaxLength(NotificationInfo.MaxNotificationNameLength)]
        public string Name { get; set; }
        /// <summary>
        /// 是否订阅
        /// </summary>
        public bool IsSubscribed { get; set; }
    }
}