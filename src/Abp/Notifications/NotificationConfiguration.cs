using Abp.Collections;

namespace Abp.Notifications
{
    /// <summary>
    /// 通知配置
    /// </summary>
    internal class NotificationConfiguration : INotificationConfiguration
    {
        /// <summary>
        /// 通知提供者列表
        /// </summary>
        public ITypeList<NotificationProvider> Providers { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public NotificationConfiguration()
        {
            Providers = new TypeList<NotificationProvider>();
        }
    }
}