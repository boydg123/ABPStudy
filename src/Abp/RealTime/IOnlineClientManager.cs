using System;
using System.Collections.Generic;

namespace Abp.RealTime
{
    /// <summary>
    /// Used to manage online clients those are connected to the application..
    /// 用于管理应用程序在线客户端
    /// </summary>
    public interface IOnlineClientManager
    {
        /// <summary>
        /// 客户端连接事件
        /// </summary>
        event EventHandler<OnlineClientEventArgs> ClientConnected;

        /// <summary>
        /// 客户端断开事件
        /// </summary>
        event EventHandler<OnlineClientEventArgs> ClientDisconnected;

        /// <summary>
        /// 用户连接事件
        /// </summary>
        event EventHandler<OnlineUserEventArgs> UserConnected;

        /// <summary>
        /// 用户断开事件
        /// </summary>
        event EventHandler<OnlineUserEventArgs> UserDisconnected;

        /// <summary>
        /// Adds a client.
        /// 添加一个客户端
        /// </summary>
        /// <param name="client">The client. / 客户端</param>
        void Add(IOnlineClient client);

        /// <summary>
        /// Removes a client by connection id.
        /// 通过连接ID移除一个客户端
        /// </summary>
        /// <param name="connectionId">The connection id. / 连接ID</param>
        /// <returns>True, if a client is removed / 如果移除则返回True</returns>
        bool Remove(string connectionId);

        /// <summary>
        /// Tries to find a client by connection id.Returns null if not found.
        /// 通过连接ID查找客户端，如果没找到则返回null
        /// </summary>
        /// <param name="connectionId">connection id / 连接ID</param>
        IOnlineClient GetByConnectionIdOrNull(string connectionId);

        /// <summary>
        /// Gets all online clients.
        /// 获取所有在线客户端
        /// </summary>
        IReadOnlyList<IOnlineClient> GetAllClients();
    }
}