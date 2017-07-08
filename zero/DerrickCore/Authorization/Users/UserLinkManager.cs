using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;

namespace Derrick.Authorization.Users
{
    /// <summary>
    /// 用户链接管理实现
    /// </summary>
    public class UserLinkManager : AbpZeroTemplateDomainServiceBase, IUserLinkManager
    {
        /// <summary>
        /// 用户帐号仓储
        /// </summary>
        private readonly IRepository<UserAccount, long> _userAccountRepository;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userAccountRepository">用户帐号仓储</param>
        public UserLinkManager(IRepository<UserAccount, long> userAccountRepository)
        {
            _userAccountRepository = userAccountRepository;
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="firstUser">第一个用户</param>
        /// <param name="secondUser">第二个用户</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task Link(User firstUser, User secondUser)
        {
            var firstUserAccount = await GetUserAccountAsync(firstUser.ToUserIdentifier());
            var secondUserAccount = await GetUserAccountAsync(secondUser.ToUserIdentifier());

            var userLinkId = firstUserAccount.UserLinkId ?? firstUserAccount.Id;
            firstUserAccount.UserLinkId = userLinkId;

            var userAccountsToLink = secondUserAccount.UserLinkId.HasValue
                ? _userAccountRepository.GetAllList(ua => ua.UserLinkId == secondUserAccount.UserLinkId.Value)
                : new List<UserAccount> { secondUserAccount };

            userAccountsToLink.ForEach(u =>
            {
                u.UserLinkId = userLinkId;
            });

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 用户连接
        /// </summary>
        /// <param name="firstUserIdentifier">第一个用户标识</param>
        /// <param name="secondUserIdentifier">第二个用户标识</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<bool> AreUsersLinked(UserIdentifier firstUserIdentifier, UserIdentifier secondUserIdentifier)
        {
            var firstUserAccount = await GetUserAccountAsync(firstUserIdentifier);
            var secondUserAccount = await GetUserAccountAsync(secondUserIdentifier);

            if (!firstUserAccount.UserLinkId.HasValue || !secondUserAccount.UserLinkId.HasValue)
            {
                return false;
            }

            return firstUserAccount.UserLinkId == secondUserAccount.UserLinkId;
        }

        /// <summary>
        /// 断开链接
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task Unlink(UserIdentifier userIdentifier)
        {
            var targetUserAccount = await GetUserAccountAsync(userIdentifier);
            targetUserAccount.UserLinkId = null;

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 获取用户帐号
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<UserAccount> GetUserAccountAsync(UserIdentifier userIdentifier)
        {
            return await _userAccountRepository.FirstOrDefaultAsync(ua => ua.TenantId == userIdentifier.TenantId && ua.UserId == userIdentifier.UserId);
        }
    }
}
