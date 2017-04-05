using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.Notifications
{
    /// <summary>
    /// Used to manage user notifications.
    /// 用于管理用户通知
    /// </summary>
    public interface IUserNotificationManager
    {
        /// <summary>
        /// Gets notifications for a user.
        /// 为用户获取通知
        /// </summary>
        /// <param name="user">User. / 用户</param>
        /// <param name="state">State / 状态</param>
        /// <param name="skipCount">Skip count. / 跳过数量</param>
        /// <param name="maxResultCount">Maximum result count. / 最大结果数量</param>
        Task<List<UserNotification>> GetUserNotificationsAsync(UserIdentifier user, UserNotificationState? state = null, int skipCount = 0, int maxResultCount = int.MaxValue);

        /// <summary>
        /// Gets user notification count.
        /// 获取用户通知数量
        /// </summary>
        /// <param name="user">User. / 用户</param>
        /// <param name="state">State. / 状态</param>
        Task<int> GetUserNotificationCountAsync(UserIdentifier user, UserNotificationState? state = null);

        /// <summary>
        /// Gets a user notification by given id.
        /// 为指定的租户ID获取用户通知
        /// </summary>
        /// <param name="tenantId">Tenant Id / 租户ID</param>
        /// <param name="userNotificationId">The user notification id. / 用户通知ID</param>
        Task<UserNotification> GetUserNotificationAsync(int? tenantId, Guid userNotificationId);

        /// <summary>
        /// Updates a user notification state.
        /// 更新用户通知状态
        /// </summary>
        /// <param name="tenantId">Tenant Id. / 租户ID</param>
        /// <param name="userNotificationId">The user notification id. / 用户通知ID</param>
        /// <param name="state">New state. / 状态</param>
        Task UpdateUserNotificationStateAsync(int? tenantId, Guid userNotificationId, UserNotificationState state);

        /// <summary>
        /// Updates all notification states for a user.
        /// 为用户更新所有通知状态
        /// </summary>
        /// <param name="user">User. / 用户</param>
        /// <param name="state">New state. / 状态</param>
        Task UpdateAllUserNotificationStatesAsync(UserIdentifier user, UserNotificationState state);

        /// <summary>
        /// Deletes a user notification.
        /// 删除用户通知
        /// </summary>
        /// <param name="tenantId">Tenant Id. / 租户ID</param>
        /// <param name="userNotificationId">The user notification id. / 用户通知ID</param>
        Task DeleteUserNotificationAsync(int? tenantId, Guid userNotificationId);

        /// <summary>
        /// Deletes all notifications of a user.
        /// 删除用户所有通知
        /// </summary>
        /// <param name="user">User. / 用户</param>
        Task DeleteAllUserNotificationsAsync(UserIdentifier user);
    }
}