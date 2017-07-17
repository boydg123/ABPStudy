using Abp.Notifications;
using Derrick.Dto;

namespace Derrick.Notifications.Dto
{
    /// <summary>
    /// 获取用户通知Input
    /// </summary>
    public class GetUserNotificationsInput : PagedInputDto
    {
        /// <summary>
        /// 用户通知状态
        /// </summary>
        public UserNotificationState? State { get; set; }
    }
}