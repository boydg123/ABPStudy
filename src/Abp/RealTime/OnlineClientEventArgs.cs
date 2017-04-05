using System;

namespace Abp.RealTime
{
    /// <summary>
    /// 在线客户端事件
    /// </summary>
    public class OnlineClientEventArgs : EventArgs
    {
        /// <summary>
        /// 客户端
        /// </summary>
        public IOnlineClient Client { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="client">客户端</param>
        public OnlineClientEventArgs(IOnlineClient client)
        {
            Client = client;
        }
    }
}