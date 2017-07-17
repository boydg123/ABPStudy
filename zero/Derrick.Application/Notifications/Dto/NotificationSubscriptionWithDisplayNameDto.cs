using Abp.AutoMapper;
using Abp.Notifications;

namespace Derrick.Notifications.Dto
{
    /// <summary>
    /// 带显示名的订阅通知Dto
    /// </summary>
    [AutoMapFrom(typeof(NotificationDefinition))]
    public class NotificationSubscriptionWithDisplayNameDto : NotificationSubscriptionDto
    {
        /// <summary>
        /// 显示名 
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}