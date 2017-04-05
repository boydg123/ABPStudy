using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;

namespace Abp.Notifications
{
    /// <summary>
    /// Implements  <see cref="IUserNotificationManager"/>.
    /// <see cref="IUserNotificationManager"/>的实现
    /// </summary>
    public class UserNotificationManager : IUserNotificationManager, ISingletonDependency
    {
        /// <summary>
        /// 通知存储
        /// </summary>
        private readonly INotificationStore _store;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotificationManager"/> class.
        /// 初始化<see cref="UserNotificationManager"/>类新的实例
        /// </summary>
        public UserNotificationManager(INotificationStore store)
        {
            _store = store;
        }

        /// <summary>
        /// 为用户获取通知
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="state">状态</param>
        /// <param name="skipCount">跳过数量</param>
        /// <param name="maxResultCount">最大返回数量</param>
        /// <returns></returns>
        public async Task<List<UserNotification>> GetUserNotificationsAsync(UserIdentifier user, UserNotificationState? state = null, int skipCount = 0, int maxResultCount = int.MaxValue)
        {
            var userNotifications = await _store.GetUserNotificationsWithNotificationsAsync(user, state, skipCount, maxResultCount);
            return userNotifications
                .Select(un => un.ToUserNotification())
                .ToList();
        }

        /// <summary>
        /// 获取用户通知数量
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public Task<int> GetUserNotificationCountAsync(UserIdentifier user, UserNotificationState? state = null)
        {
            return _store.GetUserNotificationCountAsync(user, state);
        }

        /// <summary>
        /// 为指定的租户ID获取用户通知
        /// </summary>
        /// <param name="tenantId">租户ID</param>
        /// <param name="userNotificationId">用户通知ID</param>
        /// <returns></returns>
        public async Task<UserNotification> GetUserNotificationAsync(int? tenantId, Guid userNotificationId)
        {
            var userNotification = await _store.GetUserNotificationWithNotificationOrNullAsync(tenantId, userNotificationId);
            if (userNotification == null)
            {
                return null;
            }

            return userNotification.ToUserNotification();
        }

        /// <summary>
        /// 更新用户通知状态
        /// </summary>
        /// <param name="tenantId">租户ID</param>
        /// <param name="userNotificationId">用户通知ID</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public Task UpdateUserNotificationStateAsync(int? tenantId, Guid userNotificationId, UserNotificationState state)
        {
            return _store.UpdateUserNotificationStateAsync(tenantId, userNotificationId, state);
        }

        /// <summary>
        /// 为用户更新所有通知状态
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public Task UpdateAllUserNotificationStatesAsync(UserIdentifier user, UserNotificationState state)
        {
            return _store.UpdateAllUserNotificationStatesAsync(user, state);
        }

        /// <summary>
        /// 删除用户通知
        /// </summary>
        /// <param name="tenantId">租户ID</param>
        /// <param name="userNotificationId">用户通知ID</param>
        /// <returns></returns>
        public Task DeleteUserNotificationAsync(int? tenantId, Guid userNotificationId)
        {
            return _store.DeleteUserNotificationAsync(tenantId, userNotificationId);
        }

        /// <summary>
        /// 删除用户所有通知
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public Task DeleteAllUserNotificationsAsync(UserIdentifier user)
        {
            return _store.DeleteAllUserNotificationsAsync(user);
        }
    }
}