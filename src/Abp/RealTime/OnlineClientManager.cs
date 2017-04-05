using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Extensions;

namespace Abp.RealTime
{
    /// <summary>
    /// Implements <see cref="IOnlineClientManager"/>.
    /// <see cref="IOnlineClientManager"/>的实现
    /// </summary>
    public class OnlineClientManager : IOnlineClientManager, ISingletonDependency
    {
        /// <summary>
        /// 客户端连接事件
        /// </summary>
        public event EventHandler<OnlineClientEventArgs> ClientConnected;

        /// <summary>
        /// 客户端断开事件
        /// </summary>
        public event EventHandler<OnlineClientEventArgs> ClientDisconnected;

        /// <summary>
        /// 用户连接事件
        /// </summary>
        public event EventHandler<OnlineUserEventArgs> UserConnected;

        /// <summary>
        /// 用户断开事件
        /// </summary>
        public event EventHandler<OnlineUserEventArgs> UserDisconnected;

        /// <summary>
        /// Online clients.
        /// 客户端字典集合
        /// </summary>
        private readonly ConcurrentDictionary<string, IOnlineClient> _clients;

        private readonly object _syncObj = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="OnlineClientManager"/> class.
        /// 初始化<see cref="OnlineClientManager"/>类新的实例
        /// </summary>
        public OnlineClientManager()
        {
            _clients = new ConcurrentDictionary<string, IOnlineClient>();
        }

        /// <summary>
        /// 添加一个客户端
        /// </summary>
        /// <param name="client">客户端</param>
        public void Add(IOnlineClient client)
        {
            lock (_syncObj)
            {
                var userWasAlreadyOnline = false;
                var user = client.ToUserIdentifierOrNull();

                if (user != null)
                {
                    userWasAlreadyOnline = this.IsOnline(user);
                }

                _clients[client.ConnectionId] = client;

                ClientConnected.InvokeSafely(this, new OnlineClientEventArgs(client));

                if (user != null && !userWasAlreadyOnline)
                {
                    UserConnected.InvokeSafely(this, new OnlineUserEventArgs(user, client));
                }
            }
        }

        /// <summary>
        /// 通过连接ID移除一个客户端
        /// </summary>
        /// <param name="connectionId">连接ID</param>
        /// <returns>如果移除则返回True</returns>
        public bool Remove(string connectionId)
        {
            lock (_syncObj)
            {
                IOnlineClient client;
                var isRemoved = _clients.TryRemove(connectionId, out client);

                if (isRemoved)
                {
                    var user = client.ToUserIdentifierOrNull();

                    if (user != null && !this.IsOnline(user))
                    {
                        UserDisconnected.InvokeSafely(this, new OnlineUserEventArgs(user, client));
                    }

                    ClientDisconnected.InvokeSafely(this, new OnlineClientEventArgs(client));
                }

                return isRemoved;
            }
        }

        /// <summary>
        /// 通过连接ID查找客户端，如果没找到则返回null
        /// </summary>
        /// <param name="connectionId">连接ID</param>
        /// <returns></returns>
        public IOnlineClient GetByConnectionIdOrNull(string connectionId)
        {
            lock (_syncObj)
            {
                return _clients.GetOrDefault(connectionId);
            }
        }

        /// <summary>
        /// 获取所有在线客户端
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<IOnlineClient> GetAllClients()
        {
            lock (_syncObj)
            {
                return _clients.Values.ToImmutableList();
            }
        }
    }
}