using Abp.Authorization;
using Abp.Localization;
using Abp.Notifications;
using Derrick.Authorization;

namespace Derrick.Notifications
{
    /// <summary>
    /// APP通知提供器
    /// </summary>
    public class AppNotificationProvider : NotificationProvider
    {
        /// <summary>
        /// 设置通知
        /// </summary>
        /// <param name="context">通知定义上下文</param>
        public override void SetNotifications(INotificationDefinitionContext context)
        {
            context.Manager.Add(
                new NotificationDefinition(
                    AppNotificationNames.NewUserRegistered,
                    displayName: L("NewUserRegisteredNotificationDefinition"),
                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Users)
                    )
                );

            context.Manager.Add(
                new NotificationDefinition(
                    AppNotificationNames.NewTenantRegistered,
                    displayName: L("NewTenantRegisteredNotificationDefinition"),
                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenants)
                    )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, AbpZeroTemplateConsts.LocalizationSourceName);
        }
    }
}
