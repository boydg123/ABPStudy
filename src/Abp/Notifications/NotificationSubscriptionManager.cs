using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Json;

namespace Abp.Notifications
{
    /// <summary>
    /// Implements <see cref="INotificationSubscriptionManager"/>.
    /// <see cref="INotificationSubscriptionManager"/>的实现
    /// </summary>
    public class NotificationSubscriptionManager : INotificationSubscriptionManager, ITransientDependency
    {
        /// <summary>
        /// 通知存储器
        /// </summary>
        private readonly INotificationStore _store;

        /// <summary>
        /// 通知定义管理器
        /// </summary>
        private readonly INotificationDefinitionManager _notificationDefinitionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationSubscriptionManager"/> class.
        /// 初始化<see cref="NotificationSubscriptionManager"/>类新的实例
        /// </summary>
        public NotificationSubscriptionManager(INotificationStore store, INotificationDefinitionManager notificationDefinitionManager)
        {
            _store = store;
            _notificationDefinitionManager = notificationDefinitionManager;
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="notificationName">通知名称</param>
        /// <param name="entityIdentifier">标识实体</param>
        /// <returns></returns>
        public async Task SubscribeAsync(UserIdentifier user, string notificationName, EntityIdentifier entityIdentifier = null)
        {
            if (await IsSubscribedAsync(user, notificationName, entityIdentifier))
            {
                return;
            }

            await _store.InsertSubscriptionAsync(
                new NotificationSubscriptionInfo(
                    user.TenantId,
                    user.UserId,
                    notificationName,
                    entityIdentifier
                    )
                );
        }

        /// <summary>
        /// 订阅所有可用的通知
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public async Task SubscribeToAllAvailableNotificationsAsync(UserIdentifier user)
        {
            var notificationDefinitions = (await _notificationDefinitionManager
                .GetAllAvailableAsync(user))
                .Where(nd => nd.EntityType == null)
                .ToList();

            foreach (var notificationDefinition in notificationDefinitions)
            {
                await SubscribeAsync(user, notificationDefinition.Name);
            }
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="notificationName">通知名称</param>
        /// <param name="entityIdentifier">实体标识</param>
        /// <returns></returns>
        public async Task UnsubscribeAsync(UserIdentifier user, string notificationName, EntityIdentifier entityIdentifier = null)
        {
            await _store.DeleteSubscriptionAsync(
                user,
                notificationName,
                entityIdentifier == null ? null : entityIdentifier.Type.FullName,
                entityIdentifier == null ? null : entityIdentifier.Id.ToJsonString()
                );
        }
        
        // TODO: Can work only for single database approach!
        /// <summary>
        /// 获取订阅
        /// </summary>
        /// <param name="notificationName">通知名称</param>
        /// <param name="entityIdentifier">实体标识</param>
        /// <returns></returns>
        public async Task<List<NotificationSubscription>> GetSubscriptionsAsync(string notificationName, EntityIdentifier entityIdentifier = null)
        {
            var notificationSubscriptionInfos = await _store.GetSubscriptionsAsync(
                notificationName,
                entityIdentifier == null ? null : entityIdentifier.Type.FullName,
                entityIdentifier == null ? null : entityIdentifier.Id.ToJsonString()
                );

            return notificationSubscriptionInfos
                .Select(nsi => nsi.ToNotificationSubscription())
                .ToList();
        }

        /// <summary>
        /// 为指定的通知获取所有订阅
        /// </summary>
        /// <param name="tenantId">租户ID，宿主则是null</param>
        /// <param name="notificationName">通知名称</param>
        /// <param name="entityIdentifier">标识实体</param>
        /// <returns></returns>
        public async Task<List<NotificationSubscription>> GetSubscriptionsAsync(int? tenantId, string notificationName, EntityIdentifier entityIdentifier = null)
        {
            var notificationSubscriptionInfos = await _store.GetSubscriptionsAsync(
                new[] { tenantId },
                notificationName,
                entityIdentifier == null ? null : entityIdentifier.Type.FullName,
                entityIdentifier == null ? null : entityIdentifier.Id.ToJsonString()
                );

            return notificationSubscriptionInfos
                .Select(nsi => nsi.ToNotificationSubscription())
                .ToList();
        }

        /// <summary>
        /// 为用户获取所有订阅通知
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public async Task<List<NotificationSubscription>> GetSubscribedNotificationsAsync(UserIdentifier user)
        {
            var notificationSubscriptionInfos = await _store.GetSubscriptionsAsync(user);

            return notificationSubscriptionInfos
                .Select(nsi => nsi.ToNotificationSubscription())
                .ToList();
        }

        /// <summary>
        /// 检查用户是否订阅了通知
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="notificationName">通知名称</param>
        /// <param name="entityIdentifier">标识实体</param>
        /// <returns></returns>
        public Task<bool> IsSubscribedAsync(UserIdentifier user, string notificationName, EntityIdentifier entityIdentifier = null)
        {
            return _store.IsSubscribedAsync(
                user,
                notificationName,
                entityIdentifier == null ? null : entityIdentifier.Type.FullName,
                entityIdentifier == null ? null : entityIdentifier.Id.ToJsonString()
                );
        }
    }
}