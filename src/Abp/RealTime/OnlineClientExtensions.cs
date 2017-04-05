using JetBrains.Annotations;

namespace Abp.RealTime
{
    /// <summary>
    /// <see cref="OnlineClient"/>扩展
    /// </summary>
    public static class OnlineClientExtensions
    {
        /// <summary>
        /// 将给定的客户端对象转换成用户标识对象
        /// </summary>
        /// <param name="onlineClient">如果有用户则返回用户，没有则返回Null</param>
        /// <returns></returns>
        [CanBeNull]
        public static UserIdentifier ToUserIdentifierOrNull(this IOnlineClient onlineClient)
        {
            return onlineClient.UserId.HasValue
                ? new UserIdentifier(onlineClient.TenantId, onlineClient.UserId.Value)
                : null;
        }
    }
}