using Abp.Notifications;
using Derrick.Dto;

namespace Derrick.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }
    }
}