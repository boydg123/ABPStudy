using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;

namespace Abp.Notifications
{
    /// <summary>
    /// Implements <see cref="INotificationStore"/> using repositories.
    /// <see cref="INotificationStore"/>的仓储实现
    /// </summary>
    public class NotificationStore : INotificationStore, ITransientDependency
    {
        /// <summary>
        /// 通知仓储
        /// </summary>
        private readonly IRepository<NotificationInfo, Guid> _notificationRepository;
        /// <summary>
        /// 商户通知仓储
        /// </summary>
        private readonly IRepository<TenantNotificationInfo, Guid> _tenantNotificationRepository;
        /// <summary>
        /// 用户通知仓储
        /// </summary>
        private readonly IRepository<UserNotificationInfo, Guid> _userNotificationRepository;
        /// <summary>
        /// 通知订阅仓储
        /// </summary>
        private readonly IRepository<NotificationSubscriptionInfo, Guid> _notificationSubscriptionRepository;
        /// <summary>
        /// 工作单元引用
        /// </summary>
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        public NotificationStore(
            IRepository<NotificationInfo, Guid> notificationRepository,
            IRepository<TenantNotificationInfo, Guid> tenantNotificationRepository,
            IRepository<UserNotificationInfo, Guid> userNotificationRepository,
            IRepository<NotificationSubscriptionInfo, Guid> notificationSubscriptionRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _notificationRepository = notificationRepository;
            _tenantNotificationRepository = tenantNotificationRepository;
            _userNotificationRepository = userNotificationRepository;
            _notificationSubscriptionRepository = notificationSubscriptionRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }
        #region 订阅
        /// <summary>
        /// 新增订阅通知
        /// </summary>
        /// <param name="subscription">通知订阅信息</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task InsertSubscriptionAsync(NotificationSubscriptionInfo subscription)
        {
            using (_unitOfWorkManager.Current.SetTenantId(subscription.TenantId))
            {
                await _notificationSubscriptionRepository.InsertAsync(subscription);
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }
        /// <summary>
        /// 删除订阅通知
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="notificationName">通知名称</param>
        /// <param name="entityTypeName">实体类型名称</param>
        /// <param name="entityId">实体ID</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task DeleteSubscriptionAsync(UserIdentifier user, string notificationName, string entityTypeName, string entityId)
        {
            using (_unitOfWorkManager.Current.SetTenantId(user.TenantId))
            {
                await _notificationSubscriptionRepository.DeleteAsync(s =>
                    s.UserId == user.UserId &&
                    s.NotificationName == notificationName &&
                    s.EntityTypeName == entityTypeName &&
                    s.EntityId == entityId
                    );
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }
        #endregion

        #region 通知
        /// <summary>
        /// 新增一个通知
        /// </summary>
        /// <param name="notification">通知消息</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task InsertNotificationAsync(NotificationInfo notification)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                await _notificationRepository.InsertAsync(notification);
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }
        /// <summary>
        /// 获取通知或Null
        /// </summary>
        /// <param name="notificationId">通知ID</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual Task<NotificationInfo> GetNotificationOrNullAsync(Guid notificationId)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                return _notificationRepository.FirstOrDefaultAsync(notificationId);
            }
        }
        #endregion

        #region 用户通知
        /// <summary>
        /// 新增用户通知
        /// </summary>
        /// <param name="userNotification">用户通知信息</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task InsertUserNotificationAsync(UserNotificationInfo userNotification)
        {
            using (_unitOfWorkManager.Current.SetTenantId(userNotification.TenantId))
            {
                await _userNotificationRepository.InsertAsync(userNotification);
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }
        #endregion 

        /// <summary>
        /// 获取订阅消息
        /// </summary>
        /// <param name="notificationName">通知名称</param>
        /// <param name="entityTypeName">实体类型名称</param>
        /// <param name="entityId">实体ID</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual Task<List<NotificationSubscriptionInfo>> GetSubscriptionsAsync(string notificationName, string entityTypeName, string entityId)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                return _notificationSubscriptionRepository.GetAllListAsync(s =>
                    s.NotificationName == notificationName &&
                    s.EntityTypeName == entityTypeName &&
                    s.EntityId == entityId
                    );
            }
        }
        /// <summary>
        /// 获取订阅消息列表
        /// </summary>
        /// <param name="tenantIds">商户ID数组</param>
        /// <param name="notificationName">通知名</param>
        /// <param name="entityTypeName">实体类型名称</param>
        /// <param name="entityId">实体ID</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<List<NotificationSubscriptionInfo>> GetSubscriptionsAsync(int?[] tenantIds, string notificationName, string entityTypeName, string entityId)
        {
            var subscriptions = new List<NotificationSubscriptionInfo>();

            foreach (var tenantId in tenantIds)
            {
                subscriptions.AddRange(await GetSubscriptionsAsync(tenantId, notificationName, entityTypeName, entityId));
            }

            return subscriptions;
        }
        /// <summary>
        /// 获取订阅消息列表
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual Task<List<NotificationSubscriptionInfo>> GetSubscriptionsAsync(UserIdentifier user)
        {
            using (_unitOfWorkManager.Current.SetTenantId(user.TenantId))
            {
                return _notificationSubscriptionRepository.GetAllListAsync(s => s.UserId == user.UserId);
            }
        }
        /// <summary>
        /// 获取订阅消息列表
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <param name="notificationName">通知名称</param>
        /// <param name="entityTypeName">实体类型名</param>
        /// <param name="entityId">实体ID</param>
        /// <returns></returns>
        [UnitOfWork]
        protected virtual Task<List<NotificationSubscriptionInfo>> GetSubscriptionsAsync(int? tenantId, string notificationName, string entityTypeName, string entityId)
        {
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                return _notificationSubscriptionRepository.GetAllListAsync(s =>
                s.NotificationName == notificationName &&
                s.EntityTypeName == entityTypeName &&
                s.EntityId == entityId
                );
            }
        }
        /// <summary>
        /// 是否是订阅消息
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="notificationName">通知名称</param>
        /// <param name="entityTypeName">实体类型名称</param>
        /// <param name="entityId">实体ID</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<bool> IsSubscribedAsync(UserIdentifier user, string notificationName, string entityTypeName, string entityId)
        {
            using (_unitOfWorkManager.Current.SetTenantId(user.TenantId))
            {
                return await _notificationSubscriptionRepository.CountAsync(s =>
                    s.UserId == user.UserId &&
                    s.NotificationName == notificationName &&
                    s.EntityTypeName == entityTypeName &&
                    s.EntityId == entityId
                    ) > 0;
            }
        }
        /// <summary>
        /// 更新用户通知状态
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <param name="userNotificationId">用户通知ID</param>
        /// <param name="state">状态信息</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task UpdateUserNotificationStateAsync(int? tenantId, Guid userNotificationId, UserNotificationState state)
        {
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var userNotification = await _userNotificationRepository.FirstOrDefaultAsync(userNotificationId);
                if (userNotification == null)
                {
                    return;
                }

                userNotification.State = state;
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }
        /// <summary>
        /// 更新所有用户通知状态
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="state">通知状态</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task UpdateAllUserNotificationStatesAsync(UserIdentifier user, UserNotificationState state)
        {
            using (_unitOfWorkManager.Current.SetTenantId(user.TenantId))
            {
                var userNotifications = await _userNotificationRepository.GetAllListAsync(un => un.UserId == user.UserId);

                foreach (var userNotification in userNotifications)
                {
                    userNotification.State = state;
                }

                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }
        /// <summary>
        /// 删除用户通知状态
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <param name="userNotificationId">用户通知ID</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task DeleteUserNotificationAsync(int? tenantId, Guid userNotificationId)
        {
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                await _userNotificationRepository.DeleteAsync(userNotificationId);
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }
        /// <summary>
        /// 删除给定用户的所有用户通知
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task DeleteAllUserNotificationsAsync(UserIdentifier user)
        {
            using (_unitOfWorkManager.Current.SetTenantId(user.TenantId))
            {
                await _userNotificationRepository.DeleteAsync(un => un.UserId == user.UserId);
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }
        /// <summary>
        /// 获取用户通知以及关联的通知
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="state">用户通知状态</param>
        /// <param name="skipCount">跳过的数量</param>
        /// <param name="maxResultCount">最大结果数</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual Task<List<UserNotificationInfoWithNotificationInfo>> GetUserNotificationsWithNotificationsAsync(UserIdentifier user, UserNotificationState? state = null, int skipCount = 0, int maxResultCount = int.MaxValue)
        {
            using (_unitOfWorkManager.Current.SetTenantId(user.TenantId))
            {
                var query = from userNotificationInfo in _userNotificationRepository.GetAll()
                            join tenantNotificationInfo in _tenantNotificationRepository.GetAll() on userNotificationInfo.TenantNotificationId equals tenantNotificationInfo.Id
                            where userNotificationInfo.UserId == user.UserId && (state == null || userNotificationInfo.State == state.Value)
                            orderby tenantNotificationInfo.CreationTime descending
                            select new { userNotificationInfo, tenantNotificationInfo = tenantNotificationInfo };

                query = query.PageBy(skipCount, maxResultCount);

                var list = query.ToList();

                return Task.FromResult(list.Select(
                    a => new UserNotificationInfoWithNotificationInfo(a.userNotificationInfo, a.tenantNotificationInfo)
                    ).ToList());
            }
        }
        /// <summary>
        /// 获取用户通知数量
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="state">用户通知状态</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual Task<int> GetUserNotificationCountAsync(UserIdentifier user, UserNotificationState? state = null)
        {
            using (_unitOfWorkManager.Current.SetTenantId(user.TenantId))
            {
                return _userNotificationRepository.CountAsync(un => un.UserId == user.UserId && (state == null || un.State == state.Value));
            }
        }
        /// <summary>
        /// 获取用户通知以及关联的通知或Null
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <param name="userNotificationId">用户通知ID</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual Task<UserNotificationInfoWithNotificationInfo> GetUserNotificationWithNotificationOrNullAsync(int? tenantId, Guid userNotificationId)
        {
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var query = from userNotificationInfo in _userNotificationRepository.GetAll()
                            join tenantNotificationInfo in _tenantNotificationRepository.GetAll() on userNotificationInfo.TenantNotificationId equals tenantNotificationInfo.Id
                            where userNotificationInfo.Id == userNotificationId
                            select new { userNotificationInfo, tenantNotificationInfo = tenantNotificationInfo };

                var item = query.FirstOrDefault();
                if (item == null)
                {
                    return Task.FromResult((UserNotificationInfoWithNotificationInfo)null);
                }

                return Task.FromResult(new UserNotificationInfoWithNotificationInfo(item.userNotificationInfo, item.tenantNotificationInfo));
            }
        }
        /// <summary>
        /// 新增商户通知
        /// </summary>
        /// <param name="tenantNotificationInfo">商户通知信息</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual Task InsertTenantNotificationAsync(TenantNotificationInfo tenantNotificationInfo)
        {
            using (_unitOfWorkManager.Current.SetTenantId(tenantNotificationInfo.TenantId))
            {
                return _tenantNotificationRepository.InsertAsync(tenantNotificationInfo);
            }
        }
        /// <summary>
        /// 删除通知
        /// </summary>
        /// <param name="notification">通知信息</param>
        /// <returns></returns>
        public virtual Task DeleteNotificationAsync(NotificationInfo notification)
        {
            return _notificationRepository.DeleteAsync(notification);
        }
    }
}
