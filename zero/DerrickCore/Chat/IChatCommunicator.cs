using System.Collections.Generic;
using Abp;
using Abp.RealTime;
using Derrick.Friendships;

namespace Derrick.Chat
{
    /// <summary>
    /// 聊天沟通接口
    /// </summary>
    public interface IChatCommunicator
    {
        /// <summary>
        /// 发送消息到客户端
        /// </summary>
        /// <param name="clients">客户端列表</param>
        /// <param name="message">消息</param>
        void SendMessageToClient(IReadOnlyList<IOnlineClient> clients, ChatMessage message);

        /// <summary>
        /// 发送好友请求到客户端
        /// </summary>
        /// <param name="clients">客户端列表</param>
        /// <param name="friend">友谊信息</param>
        /// <param name="isOwnRequest">是否是自己的请求</param>
        /// <param name="isFriendOnline">朋友是否在线</param>
        void SendFriendshipRequestToClient(IReadOnlyList<IOnlineClient> clients, Friendship friend, bool isOwnRequest, bool isFriendOnline);

        /// <summary>
        /// 将用户连接更改发送到客户端
        /// </summary>
        /// <param name="clients">客户端列表</param>
        /// <param name="user">用户标识</param>
        /// <param name="isConnected">是否连接</param>
        void SendUserConnectionChangeToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user, bool isConnected);

        /// <summary>
        /// 将用户状态更改发送到客户端
        /// </summary>
        /// <param name="clients">客户端列表</param>
        /// <param name="user">用户标识</param>
        /// <param name="newState">友谊新状态</param>
        void SendUserStateChangeToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user, FriendshipState newState);

        /// <summary>
        /// 将用户所有的维度消息读给客户
        /// </summary>
        /// <param name="clients">客户端列表</param>
        /// <param name="user">用户标识</param>
        void SendAllUnreadMessagesOfUserReadToClients(IReadOnlyList<IOnlineClient> clients, UserIdentifier user);
    }
}
