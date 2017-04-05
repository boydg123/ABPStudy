using System.Threading.Tasks;

namespace Abp.Notifications
{
    /// <summary>
    /// Interface to send real time notifications to users.
    /// 向用户发送实时通知的接口
    /// </summary>
    public interface IRealTimeNotifier
    {
        /// <summary>
        /// This method tries to deliver real time notifications to specified users.
        /// 此方法试图向指定用户提供实时通知
        /// If a user is not online, it should ignore him.
        /// 如果用户不在线，就应该忽略它
        /// </summary>
        Task SendNotificationsAsync(UserNotification[] userNotifications);
    }
}