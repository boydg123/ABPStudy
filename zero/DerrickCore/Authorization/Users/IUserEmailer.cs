using System.Threading.Tasks;
using Derrick.Chat;

namespace Derrick.Authorization.Users
{
    /// <summary>
    /// 用于给用户发送邮件
    /// </summary>
    public interface IUserEmailer
    {
        /// <summary>
        /// 发送电子邮件激活链接到用户的邮箱
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="plainPassword">
        /// 可以设置为用户的普通密码用以包含在电子邮件中
        /// </param>
        Task SendEmailActivationLinkAsync(User user, string plainPassword = null);

        /// <summary>
        /// 发送一个密码重置链接到用户的邮箱
        /// </summary>
        /// <param name="user">用户</param>
        Task SendPasswordResetLinkAsync(User user);

        /// <summary>
        /// 发送一个未读的聊天信息电子邮件到用户邮箱
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="senderUsername">发送者用户名</param>
        /// <param name="senderTenancyName">发送者商户名</param>
        /// <param name="chatMessage">聊天信息</param>
        void TryToSendChatMessageMail(User user, string senderUsername, string senderTenancyName, ChatMessage chatMessage);
    }
}
