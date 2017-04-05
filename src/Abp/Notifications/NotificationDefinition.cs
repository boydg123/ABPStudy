using System;
using System.Collections.Generic;
using Abp.Application.Features;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Localization;

namespace Abp.Notifications
{
    /// <summary>
    /// Definition for a notification.
    /// 通知定义
    /// </summary>
    public class NotificationDefinition
    {
        /// <summary>
        /// Unique name of the notification.
        /// 通知唯一名称
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Related entity type with this notification (optional).
        /// 当前通知关联的实体类型(可选)
        /// </summary>
        public Type EntityType { get; private set; }

        /// <summary>
        /// Display name of the notification.Optional.
        /// 通知的显示名,可选
        /// </summary>
        public ILocalizableString DisplayName { get; set; }

        /// <summary>
        /// Description for the notification.Optional.
        /// 通知的描述，可选
        /// </summary>
        public ILocalizableString Description { get; set; }

        /// <summary>
        /// A permission dependency. This notification will be available to a user if this dependency is satisfied.Optional.
        /// 一个权限依赖,当前通知将可用于用户，如果依赖是满意的。(可选)
        /// </summary>
        public IPermissionDependency PermissionDependency { get; set; }

        /// <summary>
        /// A feature dependency. This notification will be available to a tenant if this feature is enabled.Optional.
        /// 一个功能依赖，当前通知将可用于租户如果当前功能是启用的。可选
        /// </summary>
        public IFeatureDependency FeatureDependency { get; set; }

        /// <summary>
        /// Gets/sets arbitrary objects related to this object.
        /// 获取/设置 此对象相关的任意对象
        /// Gets null if given key does not exists.
        /// 如果给定的key不存在则获取值为null
        /// This is a shortcut for <see cref="Attributes"/> dictionary.
        /// <see cref="Attributes"/>字典的快捷方式
        /// </summary>
        /// <param name="key">Key</param>
        public object this[string key]
        {
            get { return Attributes.GetOrDefault(key); }
            set { Attributes[key] = value; }
        }

        /// <summary>
        /// Arbitrary objects related to this object.These objects must be serializable.
        /// 当前对象关联的任意对象，那些对象必须可以被序列化
        /// </summary>
        public IDictionary<string, object> Attributes { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationDefinition"/> class.
        /// 初始化<see cref="NotificationDefinition"/> 类新的实例
        /// </summary>
        /// <param name="name">Unique name of the notification. / 通知的唯一名称</param>
        /// <param name="entityType">Related entity type with this notification (optional). / 通知相关联的实体(可选)</param>
        /// <param name="displayName">Display name of the notification. / 通知的显示名称</param>
        /// <param name="description">Description for the notification / 通知的描述</param>
        /// <param name="permissionDependency">A permission dependency. This notification will be available to a user if this dependency is satisfied. / 一个权限依赖,当前通知将可用于用户，如果依赖是满意的。(可选)</param>
        /// <param name="featureDependency">A feature dependency. This notification will be available to a tenant if this feature is enabled. / 一个功能依赖，当前通知将可用于租户如果当前功能是启用的。可选</param>
        public NotificationDefinition(string name, Type entityType = null, ILocalizableString displayName = null, ILocalizableString description = null, IPermissionDependency permissionDependency = null, IFeatureDependency featureDependency = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name", "name can not be null, empty or whitespace!");
            }
            
            Name = name;
            EntityType = entityType;
            DisplayName = displayName;
            Description = description;
            PermissionDependency = permissionDependency;
            FeatureDependency = featureDependency;

            Attributes = new Dictionary<string, object>();
        }
    }
}
