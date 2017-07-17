using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Configuration;
using Abp.Notifications;
using Abp.Runtime.Session;
using Derrick.Notifications.Dto;

namespace Derrick.Notifications
{
    /// <summary>
    /// 通知服务实现
    /// </summary>
    [AbpAuthorize]
    public class NotificationAppService : AbpZeroTemplateAppServiceBase, INotificationAppService
    {
        /// <summary>
        /// 通知定义管理器
        /// </summary>
        private readonly INotificationDefinitionManager _notificationDefinitionManager;
        /// <summary>
        /// 用户通知管理器
        /// </summary>
        private readonly IUserNotificationManager _userNotificationManager;
        /// <summary>
        /// 订阅通知管理器
        /// </summary>
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="notificationDefinitionManager">通知定义管理</param>
        /// <param name="userNotificationManager">用户通知管理</param>
        /// <param name="notificationSubscriptionManager">订阅通知管理</param>
        public NotificationAppService(
            INotificationDefinitionManager notificationDefinitionManager,
            IUserNotificationManager userNotificationManager, 
            INotificationSubscriptionManager notificationSubscriptionManager)
        {
            _notificationDefinitionManager = notificationDefinitionManager;
            _userNotificationManager = userNotificationManager;
            _notificationSubscriptionManager = notificationSubscriptionManager;
        }
        /// <summary>
        /// 获取用户通知列表
        /// </summary>
        /// <param name="input">用户用户通知Input</param>
        /// <returns></returns>
        [DisableAuditing]
        public async Task<GetNotificationsOutput> GetUserNotifications(GetUserNotificationsInput input)
        {
            var totalCount = await _userNotificationManager.GetUserNotificationCountAsync(
                AbpSession.ToUserIdentifier(), input.State
                );

            var unreadCount = await _userNotificationManager.GetUserNotificationCountAsync(
                AbpSession.ToUserIdentifier(), UserNotificationState.Unread
                );

            var notifications = await _userNotificationManager.GetUserNotificationsAsync(
                AbpSession.ToUserIdentifier(), input.State, input.SkipCount, input.MaxResultCount
                );

            return new GetNotificationsOutput(totalCount, unreadCount, notifications);
        }
        /// <summary>
        /// 设置所有通知为已读
        /// </summary>
        /// <returns></returns>
        public async Task SetAllNotificationsAsRead()
        {
            await _userNotificationManager.UpdateAllUserNotificationStatesAsync(AbpSession.ToUserIdentifier(), UserNotificationState.Read);
        }
        /// <summary>
        /// 设置通知为已读
        /// </summary>
        /// <param name="input">实体Dto</param>
        /// <returns></returns>
        public async Task SetNotificationAsRead(EntityDto<Guid> input)
        {
            var userNotification = await _userNotificationManager.GetUserNotificationAsync(AbpSession.TenantId, input.Id);
            if (userNotification.UserId != AbpSession.GetUserId())
            {
                throw new ApplicationException(string.Format("Given user notification id ({0}) is not belong to the current user ({1})", input.Id, AbpSession.GetUserId()));
            }

            await _userNotificationManager.UpdateUserNotificationStateAsync(AbpSession.TenantId, input.Id, UserNotificationState.Read);
        }
        /// <summary>
        /// 获取通知设置列表
        /// </summary>
        /// <returns></returns>
        public async Task<GetNotificationSettingsOutput> GetNotificationSettings()
        {
            var output = new GetNotificationSettingsOutput();
            
            output.ReceiveNotifications = await SettingManager.GetSettingValueAsync<bool>(NotificationSettingNames.ReceiveNotifications);
            
            output.Notifications = (await _notificationDefinitionManager
                .GetAllAvailableAsync(AbpSession.ToUserIdentifier()))
                .Where(nd => nd.EntityType == null) //Get general notifications, not entity related notifications.
                .MapTo<List<NotificationSubscriptionWithDisplayNameDto>>();
            
            var subscribedNotifications = (await _notificationSubscriptionManager
                .GetSubscribedNotificationsAsync(AbpSession.ToUserIdentifier()))
                .Select(ns => ns.NotificationName)
                .ToList();

            output.Notifications.ForEach(n => n.IsSubscribed = subscribedNotifications.Contains(n.Name));

            return output;
        }
        /// <summary>
        /// 更新通知设置Input
        /// </summary>
        /// <param name="input">更新通知设置Input</param>
        /// <returns></returns>
        public async Task UpdateNotificationSettings(UpdateNotificationSettingsInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), NotificationSettingNames.ReceiveNotifications, input.ReceiveNotifications.ToString());

            foreach (var notification in input.Notifications)
            {
                if (notification.IsSubscribed)
                {
                    await _notificationSubscriptionManager.SubscribeAsync(AbpSession.ToUserIdentifier(), notification.Name);
                }
                else
                {
                    await _notificationSubscriptionManager.UnsubscribeAsync(AbpSession.ToUserIdentifier(), notification.Name);
                }
            }
        }
    }
}