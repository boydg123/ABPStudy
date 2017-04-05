using System;
using System.Threading.Tasks;
using Abp.Domain.Services;

namespace Abp.Notifications
{
    /// <summary>
    /// Used to distribute notifications to users.
    /// 用于分发通知给用户
    /// </summary>
    public interface INotificationDistributer : IDomainService
    {
        /// <summary>
        /// Distributes given notification to users.
        /// 分发通知给用户
        /// </summary>
        /// <param name="notificationId">The notification id. / 通知ID</param>
        Task DistributeAsync(Guid notificationId);
    }
}