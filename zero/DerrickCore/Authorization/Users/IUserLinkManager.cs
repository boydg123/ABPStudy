using System.Threading.Tasks;
using Abp;
using Abp.Authorization.Users;

namespace Derrick.Authorization.Users
{
    /// <summary>
    /// 用户链接管理器
    /// </summary>
    public interface IUserLinkManager
    {
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="firstUser">第一个用户</param>
        /// <param name="secondUser">第二个用户</param>
        /// <returns></returns>
        Task Link(User firstUser, User secondUser);
        /// <summary>
        /// 用户连接
        /// </summary>
        /// <param name="firstUserIdentifier">第一个用户标识</param>
        /// <param name="secondUserIdentifier">第二个用户标识</param>
        /// <returns></returns>
        Task<bool> AreUsersLinked(UserIdentifier firstUserIdentifier, UserIdentifier secondUserIdentifier);
        /// <summary>
        /// 断开链接
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <returns></returns>
        Task Unlink(UserIdentifier userIdentifier);
        /// <summary>
        /// 获取用户帐号
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <returns></returns>
        Task<UserAccount> GetUserAccountAsync(UserIdentifier userIdentifier);
    }
}