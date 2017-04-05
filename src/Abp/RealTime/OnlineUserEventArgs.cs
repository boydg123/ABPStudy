namespace Abp.RealTime
{
    /// <summary>
    /// 在线客户事件
    /// </summary>
    public class OnlineUserEventArgs : OnlineClientEventArgs
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public UserIdentifier User { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="user">用户标识</param>
        /// <param name="client">客户端</param>
        public OnlineUserEventArgs(UserIdentifier user,IOnlineClient client) 
            : base(client)
        {
            User = user;
        }
    }
}