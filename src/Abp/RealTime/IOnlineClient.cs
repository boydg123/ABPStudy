using System;
using System.Collections.Generic;

namespace Abp.RealTime
{
    /// <summary>
    /// Represents an online client connected to the application.
    /// 表示一个在线客户端连接到应用程序
    /// </summary>
    public interface IOnlineClient
    {
        /// <summary>
        /// Unique connection Id for this client.
        /// 当前客户端的连接唯一ID
        /// </summary>
        string ConnectionId { get; }

        /// <summary>
        /// IP address of this client.
        /// 客户端IP地址
        /// </summary>
        string IpAddress { get; }

        /// <summary>
        /// Tenant Id.
        /// 租户ID
        /// </summary>
        int? TenantId { get; }

        /// <summary>
        /// User Id.
        /// 用户ID
        /// </summary>
        long? UserId { get; }

        /// <summary>
        /// Connection establishment time for this client.
        /// 客户端连接建立时间
        /// </summary>
        DateTime ConnectTime { get; }

        /// <summary>
        /// Shortcut to set/get <see cref="Properties"/>.
        /// 获取/设置 <see cref="Properties"/>的快捷方式
        /// </summary>
        object this[string key] { get; set; }

        /// <summary>
        /// Can be used to add custom properties for this client.
        /// 用于客户端添加自定义属性
        /// </summary>
        Dictionary<string, object> Properties { get; }
    }
}