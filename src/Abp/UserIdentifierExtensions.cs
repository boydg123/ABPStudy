namespace Abp
{
    /// <summary>
    /// Extension methods for <see cref="UserIdentifier"/> and <see cref="IUserIdentifier"/>.
    /// <see cref="UserIdentifier"/> 和 <see cref="IUserIdentifier"/>的扩展方法
    /// </summary>
    public static class UserIdentifierExtensions
    {
        /// <summary>
        /// Creates a new <see cref="UserIdentifier"/> object from any object implements <see cref="IUserIdentifier"/>.
        /// 创建一个新的<see cref="UserIdentifier"/>对象从任何继承自<see cref="IUserIdentifier"/>的对象
        /// </summary>
        /// <param name="userIdentifier">User identifier. / 用户标识</param>
        public static UserIdentifier ToUserIdentifier(this IUserIdentifier userIdentifier)
        {
            return new UserIdentifier(userIdentifier.TenantId, userIdentifier.UserId);
        }
    }
}