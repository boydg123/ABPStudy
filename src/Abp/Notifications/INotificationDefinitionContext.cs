namespace Abp.Notifications
{
    /// <summary>
    /// Used as a context while defining notifications.
    /// 定义通知时用来当作上下文
    /// </summary>
    public interface INotificationDefinitionContext
    {
        /// <summary>
        /// Gets the notification definition manager.
        /// 获取通知定义管理器
        /// </summary>
        INotificationDefinitionManager Manager { get; }
    }
}