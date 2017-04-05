using Abp.Collections;

namespace Abp.Notifications
{
    /// <summary>
    /// Used to configure notification system.
    /// 用于配置通知系统
    /// </summary>
    public interface INotificationConfiguration
    {
        /// <summary>
        /// Notification providers.
        /// 通知提供者
        /// </summary>
        ITypeList<NotificationProvider> Providers { get; }
    }
}