using System.Threading.Tasks;

namespace Abp.Notifications
{
    /// <summary>
    /// Null pattern implementation of <see cref="IRealTimeNotifier"/>.
    /// <see cref="IRealTimeNotifier"/>null模式的实现
    /// </summary>
    public class NullRealTimeNotifier : IRealTimeNotifier
    {
        /// <summary>
        /// Gets single instance of <see cref="NullRealTimeNotifier"/> class.
        /// 获取<see cref="NullRealTimeNotifier"/>单个实例
        /// </summary>
        public static NullRealTimeNotifier Instance { get { return SingletonInstance; } }
        private static readonly NullRealTimeNotifier SingletonInstance = new NullRealTimeNotifier();

        /// <summary>
        /// 此方法试图向指定用户提供实时通知,如果用户不在线，就应该忽略它
        /// </summary>
        /// <param name="userNotifications"></param>
        /// <returns></returns>
        public Task SendNotificationsAsync(UserNotification[] userNotifications)
        {
            return Task.FromResult(0);
        }
    }
}