using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using Derrick.Authorization.Users.Dto;
using Derrick.MultiTenancy;

namespace Derrick.Authorization.Users
{
    /// <summary>
    /// <see cref="IUserLinkAppService"/>实现，用户链接APP服务
    /// </summary>
    [AbpAuthorize]
    public class UserLinkAppService : AbpZeroTemplateAppServiceBase, IUserLinkAppService
    {
        /// <summary>
        /// ABP登录信息结果类型帮助
        /// </summary>
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        /// <summary>
        /// 用户链接管理
        /// </summary>
        private readonly IUserLinkManager _userLinkManager;
        /// <summary>
        /// 商户仓储
        /// </summary>
        private readonly IRepository<Tenant> _tenantRepository;
        /// <summary>
        /// 用户帐号仓储
        /// </summary>
        private readonly IRepository<UserAccount, long> _userAccountRepository;
        /// <summary>
        /// 登录管理
        /// </summary>
        private readonly LogInManager _logInManager;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="abpLoginResultTypeHelper">ABP登录信息结果类型帮助</param>
        /// <param name="userLinkManager">用户链接管理</param>
        /// <param name="tenantRepository">商户仓储</param>
        /// <param name="userAccountRepository">用户帐号仓储</param>
        /// <param name="logInManager">登录管理</param>
        public UserLinkAppService(
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            IUserLinkManager userLinkManager,
            IRepository<Tenant> tenantRepository,
            IRepository<UserAccount, long> userAccountRepository, 
            LogInManager logInManager)
        {
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _userLinkManager = userLinkManager;
            _tenantRepository = tenantRepository;
            _userAccountRepository = userAccountRepository;
            _logInManager = logInManager;
        }
        /// <summary>
        /// 链接到用户
        /// </summary>
        /// <param name="linkToUserInput">用户连接Input信息</param>
        /// <returns></returns>
        public async Task LinkToUser(LinkToUserInput input)
        {
            var loginResult = await _logInManager.LoginAsync(input.UsernameOrEmailAddress, input.Password, input.TenancyName);

            if (loginResult.Result != AbpLoginResultType.Success)
            {
                throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, input.UsernameOrEmailAddress, input.TenancyName);
            }

            if (AbpSession.IsUser(loginResult.User))
            {
                throw new UserFriendlyException(L("YouCannotLinkToSameAccount"));
            }

            if (loginResult.User.ShouldChangePasswordOnNextLogin)
            {
                throw new UserFriendlyException(L("ChangePasswordBeforeLinkToAnAccount"));
            }

            await _userLinkManager.Link(GetCurrentUser(), loginResult.User);
        }
        /// <summary>
        /// 获取连接用户Dto列表(带分页)
        /// </summary>
        /// <param name="input">用户链接Input</param>
        /// <returns></returns>
        public async Task<PagedResultDto<LinkedUserDto>> GetLinkedUsers(GetLinkedUsersInput input)
        {
            var currentUserAccount = await _userLinkManager.GetUserAccountAsync(AbpSession.ToUserIdentifier());
            if (currentUserAccount == null)
            {
                return new PagedResultDto<LinkedUserDto>(0, new List<LinkedUserDto>());
            }

            var query = CreateLinkedUsersQuery(currentUserAccount, input.Sorting);
            query = query.Skip(input.SkipCount)
                        .Take(input.MaxResultCount);

            var totalCount = await query.CountAsync();
            var linkedUsers = await query.ToListAsync();

            return new PagedResultDto<LinkedUserDto>(
                totalCount,
                linkedUsers
            );
        }
        /// <summary>
        /// 获取最近使用的链接用户
        /// </summary>
        /// <returns></returns>
        [DisableAuditing]
        public async Task<ListResultDto<LinkedUserDto>> GetRecentlyUsedLinkedUsers()
        {
            var currentUserAccount = await _userLinkManager.GetUserAccountAsync(AbpSession.ToUserIdentifier());
            if (currentUserAccount == null)
            {
                return new ListResultDto<LinkedUserDto>();
            }

            var query = CreateLinkedUsersQuery(currentUserAccount, "LastLoginTime DESC");
            var recentlyUsedlinkedUsers = await query.Skip(0).Take(3).ToListAsync();

            return new ListResultDto<LinkedUserDto>(recentlyUsedlinkedUsers);
        }
        /// <summary>
        /// 用户断开链接
        /// </summary>
        /// <param name="input">断开链接用户Input</param>
        /// <returns></returns>
        public async Task UnlinkUser(UnlinkUserInput input)
        {
            var currentUserAccount = await _userLinkManager.GetUserAccountAsync(AbpSession.ToUserIdentifier());

            if (!currentUserAccount.UserLinkId.HasValue)
            {
                throw new ApplicationException(L("You are not linked to any account"));
            }

            if (!await _userLinkManager.AreUsersLinked(AbpSession.ToUserIdentifier(), input.ToUserIdentifier()))
            {
                return;
            }

            await _userLinkManager.Unlink(input.ToUserIdentifier());
        }
        /// <summary>
        /// 创建链接用户查询
        /// </summary>
        /// <param name="currentUserAccount">当前用户帐号</param>
        /// <param name="sorting">排序</param>
        /// <returns></returns>
        private IQueryable<LinkedUserDto> CreateLinkedUsersQuery(UserAccount currentUserAccount, string sorting)
        {
            var currentUserIdentifier = AbpSession.ToUserIdentifier();

            return (from userAccount in _userAccountRepository.GetAll()
                    join tenant in _tenantRepository.GetAll() on userAccount.TenantId equals tenant.Id into tenantJoined
                    from tenant in tenantJoined.DefaultIfEmpty()
                    where
                        (userAccount.TenantId != currentUserIdentifier.TenantId ||
                        userAccount.UserId != currentUserIdentifier.UserId) &&
                        userAccount.UserLinkId.HasValue &&
                        userAccount.UserLinkId == currentUserAccount.UserLinkId
                    select new LinkedUserDto
                    {
                        Id = userAccount.UserId,
                        TenantId = userAccount.TenantId,
                        TenancyName = tenant == null ? "." : tenant.TenancyName,
                        Username = userAccount.UserName,
                        LastLoginTime = userAccount.LastLoginTime
                    }).OrderBy(sorting);
        }
    }
}