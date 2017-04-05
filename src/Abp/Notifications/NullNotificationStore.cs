using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.Notifications
{
    /// <summary>
    /// Null pattern implementation of <see cref="INotificationStore"/>.
    /// <see cref="INotificationStore"/>的空模式实现
    /// </summary>
    public class NullNotificationStore : INotificationStore
    {
        /// <summary>
        /// 插入一个订阅通知 - 异步
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        public Task InsertSubscriptionAsync(NotificationSubscriptionInfo subscription)
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// 删除一个订阅通知 - 异步
        /// </summary>
        /// <param name="user"></param>
        /// <param name="notificationName"></param>
        /// <param name="entityTypeName"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public Task DeleteSubscriptionAsync(UserIdentifier user, string notificationName, string entityTypeName, string entityId)
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// 插入一个通知 - 异步
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        public Task InsertNotificationAsync(NotificationInfo notification)
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// 通过ID获取通知，如果没有找到则返回null
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        public Task<NotificationInfo> GetNotificationOrNullAsync(Guid notificationId)
        {
            return Task.FromResult(null as NotificationInfo);
        }

        /// <summary>
        /// 插入一个用户通知
        /// </summary>
        /// <param name="userNotification"></param>
        /// <returns></returns>
        public Task InsertUserNotificationAsync(UserNotificationInfo userNotification)
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// 获取通知订阅
        /// </summary>
        /// <param name="notificationName"></param>
        /// <param name="entityTypeName"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public Task<List<NotificationSubscriptionInfo>> GetSubscriptionsAsync(string notificationName, string entityTypeName = null, string entityId = null)
        {
            return Task.FromResult(new List<NotificationSubscriptionInfo>());
        }

        /// <summary>
        /// 为指定的用户获取通知订阅
        /// </summary>
        /// <param name="tenantIds"></param>
        /// <param name="notificationName"></param>
        /// <param name="entityTypeName"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public Task<List<NotificationSubscriptionInfo>> GetSubscriptionsAsync(int?[] tenantIds, string notificationName, string entityTypeName, string entityId)
        {
            return Task.FromResult(new List<NotificationSubscriptionInfo>());
        }

        /// <summary>
        /// 获取用户的订阅
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<List<NotificationSubscriptionInfo>> GetSubscriptionsAsync(UserIdentifier user)
        {
            return Task.FromResult(new List<NotificationSubscriptionInfo>());
        }

        /// <summary>
        /// 检查用户是否订阅了通知
        /// </summary>
        /// <param name="user"></param>
        /// <param name="notificationName"></param>
        /// <param name="entityTypeName"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public Task<bool> IsSubscribedAsync(UserIdentifier user, string notificationName, string entityTypeName, string entityId)
        {
            return Task.FromResult(false);
        }

        /// <summary>
        /// 更新用户通知状态
        /// </summary>
        /// <param name="notificationId"></param>
        /// <param name="userNotificationId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public Task UpdateUserNotificationStateAsync(int? notificationId, Guid userNotificationId, UserNotificationState state)
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// 更新用户的所有通知状态
        /// </summary>
        /// <param name="user"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public Task UpdateAllUserNotificationStatesAsync(UserIdentifier user, UserNotificationState state)
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// 删除用户通知
        /// </summary>
        /// <param name="notificationId"></param>
        /// <param name="userNotificationId"></param>
        /// <returns></returns>
        public Task DeleteUserNotificationAsync(int? notificationId, Guid userNotificationId)
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// 删除所有用户的通知
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task DeleteAllUserNotificationsAsync(UserIdentifier user)
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// 被通知的用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="state"></param>
        /// <param name="skipCount"></param>
        /// <param name="maxResultCount"></param>
        /// <returns></returns>
        public Task<List<UserNotificationInfoWithNotificationInfo>> GetUserNotificationsWithNotificationsAsync(UserIdentifier user, UserNotificationState? state = null, int skipCount = 0, int maxResultCount = int.MaxValue)
        {
            return Task.FromResult(new List<UserNotificationInfoWithNotificationInfo>());
        }

        /// <summary>
        /// 获取用户通知的数量
        /// </summary>
        /// <param name="user"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public Task<int> GetUserNotificationCountAsync(UserIdentifier user, UserNotificationState? state = null)
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// 获取用户的通知
        /// </summary>
        /// <param name="tenantId">租户ID</param>
        /// <param name="userNotificationId">跳过的数量</param>
        /// <returns></returns>
        public Task<UserNotificationInfoWithNotificationInfo> GetUserNotificationWithNotificationOrNullAsync(int? tenantId, Guid userNotificationId)
        {
            return Task.FromResult((UserNotificationInfoWithNotificationInfo)null);
        }

        /// <summary>
        /// 为租户插入通知
        /// </summary>
        /// <param name="tenantNotificationInfo"></param>
        /// <returns></returns>
        public Task InsertTenantNotificationAsync(TenantNotificationInfo tenantNotificationInfo)
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// 删除通知
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        public Task DeleteNotificationAsync(NotificationInfo notification)
        {
            return Task.FromResult(0);
        }
    }
}