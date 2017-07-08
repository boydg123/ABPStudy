using System;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.IdentityFramework;
using Abp.Localization;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Threading;
using Derrick.Authorization.Roles;
using Derrick.Identity;

namespace Derrick.Authorization.Users
{
    /// <summary>
    /// Extends <see cref="AbpUserManager{TRole,TUser}"/>的扩展，用户管理，用于为用户实现域逻辑
    /// </summary>
    public class UserManager : AbpUserManager<Role, User>
    {
        /// <summary>
        /// 工作单元管理器引用
        /// </summary>
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UserManager(
            UserStore userStore, 
            RoleManager roleManager, 
            IPermissionManager permissionManager, 
            IUnitOfWorkManager unitOfWorkManager, 
            ICacheManager cacheManager, 
            IRepository<OrganizationUnit, long> organizationUnitRepository, 
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository, 
            IOrganizationUnitSettings organizationUnitSettings,
            IdentityEmailMessageService emailService,
            ILocalizationManager localizationManager,
            ISettingManager settingManager,
            IdentitySmsMessageService smsService,
            IUserTokenProviderAccessor userTokenProviderAccessor) 
            : base(
                  userStore, 
                  roleManager, 
                  permissionManager, 
                  unitOfWorkManager, 
                  cacheManager, 
                  organizationUnitRepository, 
                  userOrganizationUnitRepository, 
                  organizationUnitSettings,
                  localizationManager,
                  emailService,
                  settingManager,
                  userTokenProviderAccessor)
        {
            _unitOfWorkManager = unitOfWorkManager;

            SmsService = smsService;
        }

        /// <summary>
        /// 获取用户或Null - 异步
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <returns></returns>
        public async Task<User> GetUserOrNullAsync(UserIdentifier userIdentifier)
        {
            using (_unitOfWorkManager.Begin())
            {
                using (_unitOfWorkManager.Current.SetTenantId(userIdentifier.TenantId))
                {
                    return await FindByIdAsync(userIdentifier.UserId);
                }
            }
        }

        /// <summary>
        /// 获取用户或Null
        /// </summary>
        /// <param name="userIdentifier">用户</param>
        /// <returns></returns>
        public User GetUserOrNull(UserIdentifier userIdentifier)
        {
            return AsyncHelper.RunSync(() => GetUserOrNullAsync(userIdentifier));
        }

        /// <summary>
        /// 获取用户 - 异步
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <returns></returns>
        public async Task<User> GetUserAsync(UserIdentifier userIdentifier)
        {
            var user = await GetUserOrNullAsync(userIdentifier);
            if (user == null)
            {
                throw new ApplicationException("There is no user: " + userIdentifier);
            }

            return user;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userIdentifier">用户标识</param>
        /// <returns></returns>
        public User GetUser(UserIdentifier userIdentifier)
        {
            return AsyncHelper.RunSync(() => GetUserAsync(userIdentifier));
        }
    }
}