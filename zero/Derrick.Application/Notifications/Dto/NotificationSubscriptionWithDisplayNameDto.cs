using Abp.AutoMapper;
using Abp.Notifications;

namespace Derrick.Notifications.Dto
{
    [AutoMapFrom(typeof(NotificationDefinition))]
    public class NotificationSubscriptionWithDisplayNameDto : NotificationSubscriptionDto
    {
        public string DisplayName { get; set; }

        public string Description { get; set; }
    }
}