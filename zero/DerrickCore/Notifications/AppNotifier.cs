using System.Threading.Tasks;
using Abp;
using Abp.Localization;
using Abp.Notifications;
using Derrick.Authorization.Users;
using Derrick.MultiTenancy;

namespace Derrick.Notifications
{
    /// <summary>
    /// <see cref="IAppNotifier"/>实现，APP通知器
    /// </summary>
    public class AppNotifier : AbpZeroTemplateDomainServiceBase, IAppNotifier
    {
        /// <summary>
        /// 通知发布器
        /// </summary>
        private readonly INotificationPublisher _notificationPublisher;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="notificationPublisher">通知发布器</param>
        public AppNotifier(INotificationPublisher notificationPublisher)
        {
            _notificationPublisher = notificationPublisher;
        }

        /// <summary>
        /// 欢迎来到应用程序
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <returns></returns>
        public async Task WelcomeToTheApplicationAsync(User user)
        {
            await _notificationPublisher.PublishAsync(
                AppNotificationNames.WelcomeToTheApplication,
                new MessageNotificationData(L("WelcomeToTheApplicationNotificationMessage")),
                severity: NotificationSeverity.Success,
                userIds: new[] { user.ToUserIdentifier() }
                );
        }
        /// <summary>
        /// 新用户注册
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <returns></returns>
        public async Task NewUserRegisteredAsync(User user)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "NewUserRegisteredNotificationMessage",
                    AbpZeroTemplateConsts.LocalizationSourceName
                    )
                );

            notificationData["userName"] = user.UserName;
            notificationData["emailAddress"] = user.EmailAddress;

            await _notificationPublisher.PublishAsync(AppNotificationNames.NewUserRegistered, notificationData, tenantIds: new[] { user.TenantId });
        }
        /// <summary>
        /// 新商户注册
        /// </summary>
        /// <param name="tenant">商户对象</param>
        /// <returns></returns>
        public async Task NewTenantRegisteredAsync(Tenant tenant)
        {
            var notificationData = new LocalizableMessageNotificationData(
                new LocalizableString(
                    "NewTenantRegisteredNotificationMessage",
                    AbpZeroTemplateConsts.LocalizationSourceName
                    )
                );

            notificationData["tenancyName"] = tenant.TenancyName;
            await _notificationPublisher.PublishAsync(AppNotificationNames.NewTenantRegistered, notificationData);
        }

        //This is for test purposes
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="user">用户标识</param>
        /// <param name="message">消息</param>
        /// <param name="severity">通知的级别</param>
        /// <returns></returns>
        public async Task SendMessageAsync(UserIdentifier user, string message, NotificationSeverity severity = NotificationSeverity.Info)
        {
            await _notificationPublisher.PublishAsync(
                "App.SimpleMessage",
                new MessageNotificationData(message),
                severity: severity,
                userIds: new[] { user }
                );
        }
    }
}