using System;
using System.Collections.Generic;
using Abp.Json;
using Abp.Timing;

namespace Abp.RealTime
{
    /// <summary>
    /// Implements <see cref="IOnlineClient"/>.
    /// <see cref="IOnlineClient"/>的实现
    /// </summary>
    [Serializable]
    public class OnlineClient : IOnlineClient
    {
        /// <summary>
        /// Unique connection Id for this client.
        /// 当前客户端的连接唯一ID
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// IP address of this client.
        /// 客户端IP地址
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Tenant Id.
        /// 租户ID
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// User Id.
        /// 用户ID
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// Connection establishment time for this client.
        /// 客户端连接建立时间
        /// </summary>
        public DateTime ConnectTime { get; set; }

        /// <summary>
        /// Shortcut to set/get <see cref="Properties"/>.
        /// 获取/设置 <see cref="Properties"/>的快捷方式
        /// </summary>
        public object this[string key]
        {
            get { return Properties[key]; }
            set { Properties[key] = value; }
        }

        /// <summary>
        /// Can be used to add custom properties for this client.
        /// 用于客户端添加自定义属性
        /// </summary>
        public Dictionary<string, object> Properties
        {
            get { return _properties; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                _properties = value;
            }
        }
        private Dictionary<string, object> _properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnlineClient"/> class.
        /// 初始化 <see cref="OnlineClient"/> 类新的实例
        /// </summary>
        public OnlineClient()
        {
            ConnectTime = Clock.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OnlineClient"/> class.
        /// 初始化 <see cref="OnlineClient"/> 类新的实例
        /// </summary>
        /// <param name="connectionId">The connection identifier. / 连接标识ID</param>
        /// <param name="ipAddress">The ip address. / IP地址</param>
        /// <param name="tenantId">The tenant identifier. / 租户标识ID</param>
        /// <param name="userId">The user identifier. / 用户标识ID</param>
        public OnlineClient(string connectionId, string ipAddress, int? tenantId, long? userId)
            : this()
        {
            ConnectionId = connectionId;
            IpAddress = ipAddress;
            TenantId = tenantId;
            UserId = userId;

            Properties = new Dictionary<string, object>();
        }

        public override string ToString()
        {
            return this.ToJsonString();
        }
    }
}