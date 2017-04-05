using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.Notifications
{
    /// <summary>
    /// Used to manage notification definitions.
    /// 通知定义管理器
    /// </summary>
    public interface INotificationDefinitionManager
    {
        /// <summary>
        /// Adds the specified notification definition.
        /// 添加指定的通知定义
        /// </summary>
        void Add(NotificationDefinition notificationDefinition);

        /// <summary>
        /// Gets a notification definition by name.
        /// 通过名称获取通知定义
        /// Throws exception if there is no notification definition with given name.
        /// 如果没有找到则抛出异常
        /// </summary>
        NotificationDefinition Get(string name);

        /// <summary>
        /// Gets a notification definition by name.
        /// 通过给定名称获取通知定义
        /// Returns null if there is no notification definition with given name.
        /// 如果没有找到则返回Null
        /// </summary>
        NotificationDefinition GetOrNull(string name);

        /// <summary>
        /// Gets all notification definitions.
        /// 获取所有的通知定义
        /// </summary>
        IReadOnlyList<NotificationDefinition> GetAll();

        /// <summary>
        /// Checks if given notification (<see cref="name"/>) is available for given user.
        /// 为指定用户检查给定通知(<see cref="name"/>)是否可用
        /// </summary>
        Task<bool> IsAvailableAsync(string name, UserIdentifier user);

        /// <summary>
        /// Gets all available notification definitions for given user.
        /// 为指定用户获取所有可用的通知定义
        /// </summary>
        /// <param name="user">User.</param>
        Task<IReadOnlyList<NotificationDefinition>> GetAllAvailableAsync(UserIdentifier user);
    }
}