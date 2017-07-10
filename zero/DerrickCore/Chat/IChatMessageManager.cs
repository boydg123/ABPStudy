using System;
using Abp;
using Abp.Domain.Services;

namespace Derrick.Chat
{
    /// <summary>
    /// 聊天消息管理器
    /// </summary>
    public interface IChatMessageManager : IDomainService
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sender">发送人标识</param>
        /// <param name="receiver">接收人标识</param>
        /// <param name="message">消息</param>
        /// <param name="senderTenancyName">发送者商户名称</param>
        /// <param name="senderUserName">发送者用户名</param>
        /// <param name="senderProfilePictureId">发送人图片</param>
        void SendMessage(UserIdentifier sender, UserIdentifier receiver, string message, string senderTenancyName, string senderUserName, Guid? senderProfilePictureId);

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="message">聊天消息</param>
        /// <returns></returns>
        long Save(ChatMessage message);

        /// <summary>
        /// 获取未读消息数量
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <param name="sender">发送者标识</param>
        /// <returns></returns>
        int GetUnreadMessageCount(UserIdentifier userIdentifier, UserIdentifier sender);
    }
}
