using Abp.Dependency;

namespace Abp.Notifications
{
    /// <summary>
    /// This class should be implemented in order to define notifications.
    /// 此类应被实现于那些为了定义通知的类
    /// </summary>
    public abstract class NotificationProvider : ITransientDependency
    {
        /// <summary>
        /// Used to add/manipulate notification definitions.
        /// 用于添加/操纵通知定义
        /// 
        /// </summary>
        /// <param name="context">Context / 通知定义上下文</param>
        public abstract void SetNotifications(INotificationDefinitionContext context);
    }
}