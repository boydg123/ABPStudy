using System.Threading.Tasks;
using Abp;
using Abp.Notifications;
using Derrick.Authorization.Users;
using Derrick.MultiTenancy;

namespace Derrick.Notifications
{
    /// <summary>
    /// APP通知接口
    /// </summary>
    public interface IAppNotifier
    {
        /// <summary>
        /// 欢迎来到应用程序
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <returns></returns>
        Task WelcomeToTheApplicationAsync(User user);
        /// <summary>
        /// 新用户注册
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <returns></returns>
        Task NewUserRegisteredAsync(User user);
        /// <summary>
        /// 新商户注册
        /// </summary>
        /// <param name="tenant">商户对象</param>
        /// <returns></returns>
        Task NewTenantRegisteredAsync(Tenant tenant);
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="user">用户标识</param>
        /// <param name="message">消息</param>
        /// <param name="severity">通知的级别</param>
        /// <returns></returns>
        Task SendMessageAsync(UserIdentifier user, string message, NotificationSeverity severity = NotificationSeverity.Info);
    }
}
