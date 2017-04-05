﻿using System;
using Abp.Domain.Entities;
using Abp.Extensions;
using Newtonsoft.Json;

namespace Abp.Notifications
{
    /// <summary>
    /// Extension methods for <see cref="NotificationSubscriptionInfo"/>.
    /// <see cref="NotificationSubscriptionInfo"/>的扩展方法
    /// </summary>
    public static class NotificationSubscriptionInfoExtensions
    {
        /// <summary>
        /// Converts <see cref="UserNotificationInfo"/> to <see cref="UserNotification"/>.
        /// 转换<see cref="UserNotificationInfo"/>到<see cref="UserNotification"/>.
        /// </summary>
        public static NotificationSubscription ToNotificationSubscription(this NotificationSubscriptionInfo subscriptionInfo)
        {
            var entityType = subscriptionInfo.EntityTypeAssemblyQualifiedName.IsNullOrEmpty()
                ? null
                : Type.GetType(subscriptionInfo.EntityTypeAssemblyQualifiedName);

            return new NotificationSubscription
            {
                TenantId = subscriptionInfo.TenantId,
                UserId = subscriptionInfo.UserId,
                NotificationName = subscriptionInfo.NotificationName,
                EntityType = entityType,
                EntityTypeName = subscriptionInfo.EntityTypeName,
                EntityId = subscriptionInfo.EntityId.IsNullOrEmpty() ? null : JsonConvert.DeserializeObject(subscriptionInfo.EntityId, EntityHelper.GetPrimaryKeyType(entityType)),
                CreationTime = subscriptionInfo.CreationTime
            };
        }
    }
}