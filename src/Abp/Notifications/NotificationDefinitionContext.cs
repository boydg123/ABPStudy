namespace Abp.Notifications
{
    /// <summary>
    /// 通知定义上下文
    /// </summary>
    internal class NotificationDefinitionContext : INotificationDefinitionContext
    {
        /// <summary>
        /// 通知定义管理器
        /// </summary>
        public INotificationDefinitionManager Manager { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manager">通知定义管理器</param>
        public NotificationDefinitionContext(INotificationDefinitionManager manager)
        {
            Manager = manager;
        }
    }
}