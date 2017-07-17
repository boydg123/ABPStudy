using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Notifications;

namespace Derrick.Notifications.Dto
{
    /// <summary>
    /// 获取通知Output
    /// </summary>
    public class GetNotificationsOutput : PagedResultDto<UserNotification>
    {
        /// <summary>
        /// 未读数量
        /// </summary>
        public int UnreadCount { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="totalCount">总数量</param>
        /// <param name="unreadCount">未读数量</param>
        /// <param name="notifications">通知列表</param>
        public GetNotificationsOutput(int totalCount, int unreadCount, List<UserNotification> notifications)
            :base(totalCount, notifications)
        {
            UnreadCount = unreadCount;
        }
    }
}