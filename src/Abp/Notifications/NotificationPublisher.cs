using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Json;
using Abp.Runtime.Session;

namespace Abp.Notifications
{
    /// <summary>
    /// Implements <see cref="INotificationPublisher"/>.
    /// <see cref="INotificationPublisher"/>的实现
    /// </summary>
    public class NotificationPublisher : AbpServiceBase, INotificationPublisher, ITransientDependency
    {
        /// <summary>
        /// 直接分配通知的最大用户数量
        /// </summary>
        public const int MaxUserCountToDirectlyDistributeANotification = 5;

        /// <summary>
        /// Indicates all tenants.
        /// 指示所有租户
        /// </summary>
        public static int[] AllTenants
        {
            get
            {
                return new[] { NotificationInfo.AllTenantIds.To<int>() };
            }
        }

        /// <summary>
        /// Reference to ABP session.
        /// ABP Session的引用
        /// </summary>
        public IAbpSession AbpSession { get; set; }

        /// <summary>
        /// 通知存储器
        /// </summary>
        private readonly INotificationStore _store;

        /// <summary>
        /// 后台作业管理器
        /// </summary>
        private readonly IBackgroundJobManager _backgroundJobManager;

        /// <summary>
        /// 通知分发器
        /// </summary>
        private readonly INotificationDistributer _notificationDistributer;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationPublisher"/> class.
        /// 初始化 <see cref="NotificationPublisher"/>类新的实例
        /// </summary>
        public NotificationPublisher(
            INotificationStore store,
            IBackgroundJobManager backgroundJobManager,
            INotificationDistributer notificationDistributer)
        {
            _store = store;
            _backgroundJobManager = backgroundJobManager;
            _notificationDistributer = notificationDistributer;
            AbpSession = NullAbpSession.Instance;
        }

        /// <summary>
        /// 发布一个新通知
        /// </summary>
        /// <param name="notificationName">唯一的通知名称</param>
        /// <param name="data">通知数据(可选)</param>
        /// <param name="entityIdentifier">标识实体(如果通知关联了一个实体)</param>
        /// <param name="severity">通知严重程度</param>
        /// <param name="userIds">
        /// 目标用户ID,用于发送通知给明确的用户(没有检查订阅)
        /// 如果是null/空，通知发送给订阅用户
        /// </param>
        /// <param name="excludedUserIds">
        /// 排除用户ID，当向订阅用户发布通知时，可以设置为排除某些用户
        /// 如果<see cref="userIds"/>设置了则它通常不设置
        /// </param>
        /// <param name="tenantIds">
        /// 目标租户ID，用于向特定租户发送订阅用户的通知
        /// 如果<see cref="userIds"/>有值，则这个不应该被设置
        /// <see cref="NotificationPublisher.AllTenants"/>可以通过指示所有租户
        /// 但是这个方法仅仅用于多租户应用单一数据库的方法(所有租户存储在一个数据库)
        /// 当这个为null时，它将自动设置为当前租户<see cref="IAbpSession.TenantId"/>
        /// </param>
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task PublishAsync(
            string notificationName,
            NotificationData data = null,
            EntityIdentifier entityIdentifier = null,
            NotificationSeverity severity = NotificationSeverity.Info,
            UserIdentifier[] userIds = null,
            UserIdentifier[] excludedUserIds = null,
            int?[] tenantIds = null)
        {
            if (notificationName.IsNullOrEmpty())
            {
                throw new ArgumentException("NotificationName can not be null or whitespace!", "notificationName");
            }

            if (!tenantIds.IsNullOrEmpty() && !userIds.IsNullOrEmpty())
            {
                throw new ArgumentException("tenantIds can be set only if userIds is not set!", "tenantIds");
            }

            if (tenantIds.IsNullOrEmpty() && userIds.IsNullOrEmpty())
            {
                tenantIds = new[] {AbpSession.TenantId};
            }

            var notificationInfo = new NotificationInfo
            {
                NotificationName = notificationName,
                EntityTypeName = entityIdentifier == null ? null : entityIdentifier.Type.FullName,
                EntityTypeAssemblyQualifiedName = entityIdentifier == null ? null : entityIdentifier.Type.AssemblyQualifiedName,
                EntityId = entityIdentifier == null ? null : entityIdentifier.Id.ToJsonString(),
                Severity = severity,
                UserIds = userIds.IsNullOrEmpty() ? null : userIds.Select(uid => uid.ToUserIdentifierString()).JoinAsString(","),
                ExcludedUserIds = excludedUserIds.IsNullOrEmpty() ? null : excludedUserIds.Select(uid => uid.ToUserIdentifierString()).JoinAsString(","),
                TenantIds = tenantIds.IsNullOrEmpty() ? null : tenantIds.JoinAsString(","),
                Data = data == null ? null : data.ToJsonString(),
                DataTypeName = data == null ? null : data.GetType().AssemblyQualifiedName
            };

            await _store.InsertNotificationAsync(notificationInfo);

            await CurrentUnitOfWork.SaveChangesAsync(); //To get Id of the notification

            if (userIds != null && userIds.Length <= MaxUserCountToDirectlyDistributeANotification)
            {
                //We can directly distribute the notification since there are not much receivers
                await _notificationDistributer.DistributeAsync(notificationInfo.Id);
            }
            else
            {
                //We enqueue a background job since distributing may get a long time
                await _backgroundJobManager.EnqueueAsync<NotificationDistributionJob, NotificationDistributionJobArgs>(
                    new NotificationDistributionJobArgs(
                        notificationInfo.Id
                        )
                    );
            }
        }
    }
}