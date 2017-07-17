using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Derrick.Notifications.Dto;

namespace Derrick.Notifications
{
    /// <summary>
    /// 通知服务
    /// </summary>
    public interface INotificationAppService : IApplicationService
    {
        /// <summary>
        /// 获取用户通知列表
        /// </summary>
        /// <param name="input">用户用户通知Input</param>
        /// <returns></returns>
        Task<GetNotificationsOutput> GetUserNotifications(GetUserNotificationsInput input);
        /// <summary>
        /// 设置所有通知为已读
        /// </summary>
        /// <returns></returns>
        Task SetAllNotificationsAsRead();
        /// <summary>
        /// 设置通知为已读
        /// </summary>
        /// <param name="input">实体Dto</param>
        /// <returns></returns>
        Task SetNotificationAsRead(EntityDto<Guid> input);
        /// <summary>
        /// 获取通知设置列表
        /// </summary>
        /// <returns></returns>
        Task<GetNotificationSettingsOutput> GetNotificationSettings();
        /// <summary>
        /// 更新通知设置Input
        /// </summary>
        /// <param name="input">更新通知设置Input</param>
        /// <returns></returns>
        Task UpdateNotificationSettings(UpdateNotificationSettingsInput input);
    }
}