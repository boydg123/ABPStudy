using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Abp.Application.Features;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Dependency;

namespace Abp.Notifications
{
    /// <summary>
    /// Implements <see cref="INotificationDefinitionManager"/>.
    /// <see cref="INotificationDefinitionManager"/>的实现
    /// </summary>
    internal class NotificationDefinitionManager : INotificationDefinitionManager, ISingletonDependency
    {
        /// <summary>
        /// 通知系统配置器
        /// </summary>
        private readonly INotificationConfiguration _configuration;

        /// <summary>
        /// IOC管理器
        /// </summary>
        private readonly IocManager _iocManager;

        /// <summary>
        /// 通知定义字典
        /// </summary>
        private readonly IDictionary<string, NotificationDefinition> _notificationDefinitions;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iocManager">IOC管理器</param>
        /// <param name="configuration">通知系统配置器</param>
        public NotificationDefinitionManager(
            IocManager iocManager,
            INotificationConfiguration configuration)
        {
            _configuration = configuration;
            _iocManager = iocManager;

            _notificationDefinitions = new Dictionary<string, NotificationDefinition>();
        }

        /// <summary>
        /// 通知定义初始化
        /// </summary>
        public void Initialize()
        {
            var context = new NotificationDefinitionContext(this);

            foreach (var providerType in _configuration.Providers)
            {
                _iocManager.RegisterIfNot(providerType, DependencyLifeStyle.Transient); //TODO: Needed?
                using (var provider = _iocManager.ResolveAsDisposable<NotificationProvider>(providerType))
                {
                    provider.Object.SetNotifications(context);
                }
            }
        }

        /// <summary>
        /// 添加指定的通知定义
        /// </summary>
        /// <param name="notificationDefinition"></param>
        public void Add(NotificationDefinition notificationDefinition)
        {
            if (_notificationDefinitions.ContainsKey(notificationDefinition.Name))
            {
                throw new AbpInitializationException("There is already a notification definition with given name: " + notificationDefinition.Name + ". Notification names must be unique!");
            }

            _notificationDefinitions[notificationDefinition.Name] = notificationDefinition;
        }

        /// <summary>
        /// 通过名称获取通知定义,如果没有找到则抛出异常
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public NotificationDefinition Get(string name)
        {
            var definition = GetOrNull(name);
            if (definition == null)
            {
                throw new AbpException("There is no notification definition with given name: " + name);
            }

            return definition;
        }

        /// <summary>
        /// 通过给定名称获取通知定义,如果没有找到则返回Null
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public NotificationDefinition GetOrNull(string name)
        {
            return _notificationDefinitions.GetOrDefault(name);
        }

        /// <summary>
        /// 获取所有的通知定义
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<NotificationDefinition> GetAll()
        {
            return _notificationDefinitions.Values.ToImmutableList();
        }

        /// <summary>
        /// 为指定用户检查给定通知(<see cref="name"/>)是否可用
        /// </summary>
        /// <param name="name">通知名称</param>
        /// <param name="user">指定的用户</param>
        /// <returns></returns>
        public async Task<bool> IsAvailableAsync(string name, UserIdentifier user)
        {
            var notificationDefinition = GetOrNull(name);
            if (notificationDefinition == null)
            {
                return true;
            }

            if (notificationDefinition.FeatureDependency != null)
            {
                using (var featureDependencyContext = _iocManager.ResolveAsDisposable<FeatureDependencyContext>())
                {
                    featureDependencyContext.Object.TenantId = user.TenantId;

                    if (!await notificationDefinition.FeatureDependency.IsSatisfiedAsync(featureDependencyContext.Object))
                    {
                        return false;
                    }
                }
            }

            if (notificationDefinition.PermissionDependency != null)
            {
                using (var permissionDependencyContext = _iocManager.ResolveAsDisposable<PermissionDependencyContext>())
                {
                    permissionDependencyContext.Object.User = user;

                    if (!await notificationDefinition.PermissionDependency.IsSatisfiedAsync(permissionDependencyContext.Object))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 为指定用户获取所有可用的通知定义
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public async Task<IReadOnlyList<NotificationDefinition>> GetAllAvailableAsync(UserIdentifier user)
        {
            var availableDefinitions = new List<NotificationDefinition>();

            using (var permissionDependencyContext = _iocManager.ResolveAsDisposable<PermissionDependencyContext>())
            {
                permissionDependencyContext.Object.User = user;

                using (var featureDependencyContext = _iocManager.ResolveAsDisposable<FeatureDependencyContext>())
                {
                    featureDependencyContext.Object.TenantId = user.TenantId;

                    foreach (var notificationDefinition in GetAll())
                    {
                        if (notificationDefinition.PermissionDependency != null &&
                            !await notificationDefinition.PermissionDependency.IsSatisfiedAsync(permissionDependencyContext.Object))
                        {
                            continue;
                        }

                        if (user.TenantId.HasValue &&
                            notificationDefinition.FeatureDependency != null &&
                            !await notificationDefinition.FeatureDependency.IsSatisfiedAsync(featureDependencyContext.Object))
                        {
                            continue;
                        }

                        availableDefinitions.Add(notificationDefinition);
                    }
                }
            }

            return availableDefinitions.ToImmutableList();
        }
    }
}