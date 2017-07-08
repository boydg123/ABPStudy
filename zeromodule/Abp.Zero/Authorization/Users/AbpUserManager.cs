using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Abp.Application.Features;
using Abp.Authorization.Roles;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.IdentityFramework;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Abp.Zero;
using Abp.Zero.Configuration;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// ASP.NET Identity框架的扩展 <see cref="UserManager{TUser,TKey}"/>
    /// </summary>
    public abstract class AbpUserManager<TRole, TUser>
        : UserManager<TUser, long>,
        IDomainService
        where TRole : AbpRole<TUser>, new()
        where TUser : AbpUser<TUser>
    {
        /// <summary>
        /// 用户权限存储对象
        /// </summary>
        protected IUserPermissionStore<TUser> UserPermissionStore
        {
            get
            {
                if (!(Store is IUserPermissionStore<TUser>))
                {
                    throw new AbpException("Store is not IUserPermissionStore");
                }

                return Store as IUserPermissionStore<TUser>;
            }
        }
        /// <summary>
        /// 本地化管理引用
        /// </summary>
        public ILocalizationManager LocalizationManager { get; }
        /// <summary>
        /// ABP Session引用
        /// </summary>
        public IAbpSession AbpSession { get; set; }
        /// <summary>
        /// 功能依赖上下文
        /// </summary>
        public FeatureDependencyContext FeatureDependencyContext { get; set; }
        /// <summary>
        /// 角色管理引用
        /// </summary>
        protected AbpRoleManager<TRole, TUser> RoleManager { get; }
        /// <summary>
        /// ABP 用户存储引用
        /// </summary>
        public AbpUserStore<TRole, TUser> AbpStore { get; }

        /// <summary>
        /// 权限管理引用
        /// </summary>
        private readonly IPermissionManager _permissionManager;
        /// <summary>
        /// 工作单元管理引用
        /// </summary>
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        /// <summary>
        /// 缓存管理引用
        /// </summary>
        private readonly ICacheManager _cacheManager;
        /// <summary>
        /// 组织架构仓储引用
        /// </summary>
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        /// <summary>
        /// 用户组织架构仓储引用
        /// </summary>
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        /// <summary>
        /// 组织架构设置引用
        /// </summary>
        private readonly IOrganizationUnitSettings _organizationUnitSettings;
        /// <summary>
        /// 设置引用
        /// </summary>
        private readonly ISettingManager _settingManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userStore"></param>
        /// <param name="roleManager"></param>
        /// <param name="permissionManager"></param>
        /// <param name="unitOfWorkManager"></param>
        /// <param name="cacheManager"></param>
        /// <param name="organizationUnitRepository"></param>
        /// <param name="userOrganizationUnitRepository"></param>
        /// <param name="organizationUnitSettings"></param>
        /// <param name="localizationManager"></param>
        /// <param name="emailService"></param>
        /// <param name="settingManager"></param>
        /// <param name="userTokenProviderAccessor"></param>
        protected AbpUserManager(
            AbpUserStore<TRole, TUser> userStore,
            AbpRoleManager<TRole, TUser> roleManager,
            IPermissionManager permissionManager,
            IUnitOfWorkManager unitOfWorkManager,
            ICacheManager cacheManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IOrganizationUnitSettings organizationUnitSettings,
            ILocalizationManager localizationManager,
            IdentityEmailMessageService emailService, 
            ISettingManager settingManager,
            IUserTokenProviderAccessor userTokenProviderAccessor)
            : base(userStore)
        {
            AbpStore = userStore;
            RoleManager = roleManager;
            LocalizationManager = localizationManager;
            _settingManager = settingManager;

            _permissionManager = permissionManager;
            _unitOfWorkManager = unitOfWorkManager;
            _cacheManager = cacheManager;
            _organizationUnitRepository = organizationUnitRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _organizationUnitSettings = organizationUnitSettings;

            AbpSession = NullAbpSession.Instance;

            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;
            
            EmailService = emailService;

            UserTokenProvider = userTokenProviderAccessor.GetUserTokenProviderOrNull<TUser>();
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <returns></returns>
        public override async Task<IdentityResult> CreateAsync(TUser user)
        {
            var result = await CheckDuplicateUsernameOrEmailAddressAsync(user.Id, user.UserName, user.EmailAddress);
            if (!result.Succeeded)
            {
                return result;
            }

            var tenantId = GetCurrentTenantId();
            if (tenantId.HasValue && !user.TenantId.HasValue)
            {
                user.TenantId = tenantId.Value;
            }

            return await base.CreateAsync(user);
        }

        /// <summary>
        /// 检查用户是否授予给定的权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="permissionName">权限名称</param>
        public virtual async Task<bool> IsGrantedAsync(long userId, string permissionName)
        {
            return await IsGrantedAsync(
                userId,
                _permissionManager.GetPermission(permissionName)
                );
        }

        /// <summary>
        /// 检查用户是否授予给定的权限
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="permission">权限</param>
        public virtual Task<bool> IsGrantedAsync(TUser user, Permission permission)
        {
            return IsGrantedAsync(user.Id, permission);
        }

        /// <summary>
        /// 检查用户是否授予给定的权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="permission">权限对象</param>
        public virtual async Task<bool> IsGrantedAsync(long userId, Permission permission)
        {
            //Check for multi-tenancy side
            if (!permission.MultiTenancySides.HasFlag(AbpSession.MultiTenancySide))
            {
                return false;
            }

            //Check for depended features
            if (permission.FeatureDependency != null && AbpSession.MultiTenancySide == MultiTenancySides.Tenant)
            {
                if (!await permission.FeatureDependency.IsSatisfiedAsync(FeatureDependencyContext))
                {
                    return false;
                }
            }

            //Get cached user permissions
            var cacheItem = await GetUserPermissionCacheItemAsync(userId);

            //Check for user-specific value
            if (cacheItem.GrantedPermissions.Contains(permission.Name))
            {
                return true;
            }

            if (cacheItem.ProhibitedPermissions.Contains(permission.Name))
            {
                return false;
            }

            //Check for roles
            foreach (var roleId in cacheItem.RoleIds)
            {
                if (await RoleManager.IsGrantedAsync(roleId, permission))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取用户所有的授权对象
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns>授权的列表</returns>
        public virtual async Task<IReadOnlyList<Permission>> GetGrantedPermissionsAsync(TUser user)
        {
            var permissionList = new List<Permission>();

            foreach (var permission in _permissionManager.GetAllPermissions())
            {
                if (await IsGrantedAsync(user.Id, permission))
                {
                    permissionList.Add(permission);
                }
            }

            return permissionList;
        }

        /// <summary>
        /// 立即设置用户的所有授予权限。禁止所有其他权限。
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="permissions">权限列表</param>
        public virtual async Task SetGrantedPermissionsAsync(TUser user, IEnumerable<Permission> permissions)
        {
            var oldPermissions = await GetGrantedPermissionsAsync(user);
            var newPermissions = permissions.ToArray();

            foreach (var permission in oldPermissions.Where(p => !newPermissions.Contains(p)))
            {
                await ProhibitPermissionAsync(user, permission);
            }

            foreach (var permission in newPermissions.Where(p => !oldPermissions.Contains(p)))
            {
                await GrantPermissionAsync(user, permission);
            }
        }

        /// <summary>
        /// 禁止用户的所有权限
        /// </summary>
        /// <param name="user">用户</param>
        public async Task ProhibitAllPermissionsAsync(TUser user)
        {
            foreach (var permission in _permissionManager.GetAllPermissions())
            {
                await ProhibitPermissionAsync(user, permission);
            }
        }

        /// <summary>
        /// 为用户重置所有的权限设置，它移除用户的所有权限，用户将根据自己的角色拥有权限
        /// 此方法不禁止所有权限，那使用<see cref="ProhibitAllPermissionsAsync"/>.
        /// </summary>
        /// <param name="user">User</param>
        public async Task ResetAllPermissionsAsync(TUser user)
        {
            await UserPermissionStore.RemoveAllPermissionSettingsAsync(user);
        }

        /// <summary>
        /// 如果尚未授予则授予用户指定的权限
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <param name="permission">权限对象</param>
        public virtual async Task GrantPermissionAsync(TUser user, Permission permission)
        {
            await UserPermissionStore.RemovePermissionAsync(user, new PermissionGrantInfo(permission.Name, false));

            if (await IsGrantedAsync(user.Id, permission))
            {
                return;
            }

            await UserPermissionStore.AddPermissionAsync(user, new PermissionGrantInfo(permission.Name, true));
        }

        /// <summary>
        /// 如果权限是授予的，则禁止这个权限
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <param name="permission">权限对象</param>
        public virtual async Task ProhibitPermissionAsync(TUser user, Permission permission)
        {
            await UserPermissionStore.RemovePermissionAsync(user, new PermissionGrantInfo(permission.Name, true));

            if (!await IsGrantedAsync(user.Id, permission))
            {
                return;
            }

            await UserPermissionStore.AddPermissionAsync(user, new PermissionGrantInfo(permission.Name, false));
        }
        /// <summary>
        /// 通过用户名或邮箱查找
        /// </summary>
        /// <param name="userNameOrEmailAddress">用户名或邮箱</param>
        /// <returns></returns>
        public virtual async Task<TUser> FindByNameOrEmailAsync(string userNameOrEmailAddress)
        {
            return await AbpStore.FindByNameOrEmailAsync(userNameOrEmailAddress);
        }
        /// <summary>
        /// 查找所有用户
        /// </summary>
        /// <param name="login">用户登录信息</param>
        /// <returns></returns>
        public virtual Task<List<TUser>> FindAllAsync(UserLoginInfo login)
        {
            return AbpStore.FindAllAsync(login);
        }

        /// <summary>
        /// 通过给定的ID获取一个用户，如果没有找打则抛出异常
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户对象</returns>
        /// <exception cref="AbpException">通过给定的ID没有找到用户则抛出异常</exception>
        public virtual async Task<TUser> GetUserByIdAsync(long userId)
        {
            var user = await FindByIdAsync(userId);
            if (user == null)
            {
                throw new AbpException("There is no user with id: " + userId);
            }

            return user;
        }

        /// <summary>
        /// 创建标识
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="authenticationType">认证类型</param>
        /// <returns></returns>
        public async override Task<ClaimsIdentity> CreateIdentityAsync(TUser user, string authenticationType)
        {
            var identity = await base.CreateIdentityAsync(user, authenticationType);
            if (user.TenantId.HasValue)
            {
                identity.AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.Value.ToString(CultureInfo.InvariantCulture)));
            }

            return identity;
        }
        /// <summary>
        /// 修改身份
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns>身份结果</returns>
        public async override Task<IdentityResult> UpdateAsync(TUser user)
        {
            var result = await CheckDuplicateUsernameOrEmailAddressAsync(user.Id, user.UserName, user.EmailAddress);
            if (!result.Succeeded)
            {
                return result;
            }

            var oldUserName = (await GetUserByIdAsync(user.Id)).UserName;
            if (oldUserName == AbpUser<TUser>.AdminUserName && user.UserName != AbpUser<TUser>.AdminUserName)
            {
                return AbpIdentityResult.Failed(string.Format(L("CanNotRenameAdminUser"), AbpUser<TUser>.AdminUserName));
            }

            return await base.UpdateAsync(user);
        }
        /// <summary>
        /// 删除身份
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns>身份结果</returns>
        public async override Task<IdentityResult> DeleteAsync(TUser user)
        {
            if (user.UserName == AbpUser<TUser>.AdminUserName)
            {
                return AbpIdentityResult.Failed(string.Format(L("CanNotDeleteAdminUser"), AbpUser<TUser>.AdminUserName));
            }

            return await base.DeleteAsync(user);
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="newPassword">新密码</param>
        /// <returns>身份结果</returns>
        public virtual async Task<IdentityResult> ChangePasswordAsync(TUser user, string newPassword)
        {
            var result = await PasswordValidator.ValidateAsync(newPassword);
            if (!result.Succeeded)
            {
                return result;
            }

            await AbpStore.SetPasswordHashAsync(user, PasswordHasher.HashPassword(newPassword));
            return IdentityResult.Success;
        }
        /// <summary>
        /// 检查重复的用户名或电子邮件地址分配
        /// </summary>
        /// <param name="expectedUserId">预期的用户ID</param>
        /// <param name="userName">用户UserName</param>
        /// <param name="emailAddress">邮箱地址</param>
        /// <returns></returns>
        public virtual async Task<IdentityResult> CheckDuplicateUsernameOrEmailAddressAsync(long? expectedUserId, string userName, string emailAddress)
        {
            var user = (await FindByNameAsync(userName));
            if (user != null && user.Id != expectedUserId)
            {
                return AbpIdentityResult.Failed(string.Format(L("Identity.DuplicateName"), userName));
            }

            user = (await FindByEmailAsync(emailAddress));
            if (user != null && user.Id != expectedUserId)
            {
                return AbpIdentityResult.Failed(string.Format(L("Identity.DuplicateEmail"), emailAddress));
            }

            return IdentityResult.Success;
        }

        /// <summary>
        /// 设置角色
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="roleNames">角色名</param>
        /// <returns></returns>
        public virtual async Task<IdentityResult> SetRoles(TUser user, string[] roleNames)
        {
            //Remove from removed roles
            foreach (var userRole in user.Roles.ToList())
            {
                var role = await RoleManager.FindByIdAsync(userRole.RoleId);
                if (roleNames.All(roleName => role.Name != roleName))
                {
                    var result = await RemoveFromRoleAsync(user.Id, role.Name);
                    if (!result.Succeeded)
                    {
                        return result;
                    }
                }
            }

            //Add to added roles
            foreach (var roleName in roleNames)
            {
                var role = await RoleManager.GetRoleByNameAsync(roleName);
                if (user.Roles.All(ur => ur.RoleId != role.Id))
                {
                    var result = await AddToRoleAsync(user.Id, roleName);
                    if (!result.Succeeded)
                    {
                        return result;
                    }
                }
            }

            return IdentityResult.Success;
        }
        /// <summary>
        /// 判断指定用户是否在指定的组织结构中
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="ouId">组织ID</param>
        /// <returns></returns>
        public virtual async Task<bool> IsInOrganizationUnitAsync(long userId, long ouId)
        {
            return await IsInOrganizationUnitAsync(
                await GetUserByIdAsync(userId),
                await _organizationUnitRepository.GetAsync(ouId)
                );
        }
        /// <summary>
        /// 判断用户是否在指定的组织中
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="ou">组织</param>
        /// <returns></returns>
        public virtual async Task<bool> IsInOrganizationUnitAsync(TUser user, OrganizationUnit ou)
        {
            return await _userOrganizationUnitRepository.CountAsync(uou =>
                uou.UserId == user.Id && uou.OrganizationUnitId == ou.Id
                ) > 0;
        }

        /// <summary>
        /// 将用户添加到指定组织中
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="ouId">组织ID</param>
        /// <returns></returns>
        public virtual async Task AddToOrganizationUnitAsync(long userId, long ouId)
        {
            await AddToOrganizationUnitAsync(
                await GetUserByIdAsync(userId),
                await _organizationUnitRepository.GetAsync(ouId)
                );
        }
        /// <summary>
        /// 添加用户到指定组织中
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="ou">组织</param>
        /// <returns></returns>
        public virtual async Task AddToOrganizationUnitAsync(TUser user, OrganizationUnit ou)
        {
            var currentOus = await GetOrganizationUnitsAsync(user);

            if (currentOus.Any(cou => cou.Id == ou.Id))
            {
                return;
            }

            await CheckMaxUserOrganizationUnitMembershipCountAsync(user.TenantId, currentOus.Count + 1);

            await _userOrganizationUnitRepository.InsertAsync(new UserOrganizationUnit(user.TenantId, user.Id, ou.Id));
        }
        /// <summary>
        /// 移除指定组织中的特定用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="ouId">组织ID</param>
        /// <returns></returns>
        public virtual async Task RemoveFromOrganizationUnitAsync(long userId, long ouId)
        {
            await RemoveFromOrganizationUnitAsync(
                await GetUserByIdAsync(userId),
                await _organizationUnitRepository.GetAsync(ouId)
                );
        }
        /// <summary>
        /// 移除指定组织中的特定用户
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="ou">组织</param>
        /// <returns></returns>
        public virtual async Task RemoveFromOrganizationUnitAsync(TUser user, OrganizationUnit ou)
        {
            await _userOrganizationUnitRepository.DeleteAsync(uou => uou.UserId == user.Id && uou.OrganizationUnitId == ou.Id);
        }
        /// <summary>
        /// 设置组织
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="organizationUnitIds">组织结构ID数组</param>
        /// <returns></returns>
        public virtual async Task SetOrganizationUnitsAsync(long userId, params long[] organizationUnitIds)
        {
            await SetOrganizationUnitsAsync(
                await GetUserByIdAsync(userId),
                organizationUnitIds
                );
        }
        /// <summary>
        /// 检查组织结构成员的最大成员数量
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <param name="requestedCount">请求数量</param>
        /// <returns></returns>
        private async Task CheckMaxUserOrganizationUnitMembershipCountAsync(int? tenantId, int requestedCount)
        {
            var maxCount = await _organizationUnitSettings.GetMaxUserMembershipCountAsync(tenantId);
            if (requestedCount > maxCount)
            {
                throw new AbpException(string.Format("Can not set more than {0} organization unit for a user!", maxCount));
            }
        }
        /// <summary>
        /// 设置组织结构
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <param name="organizationUnitIds">组织结构ID数组</param>
        /// <returns></returns>
        public virtual async Task SetOrganizationUnitsAsync(TUser user, params long[] organizationUnitIds)
        {
            if (organizationUnitIds == null)
            {
                organizationUnitIds = new long[0];
            }

            await CheckMaxUserOrganizationUnitMembershipCountAsync(user.TenantId, organizationUnitIds.Length);

            var currentOus = await GetOrganizationUnitsAsync(user);

            //Remove from removed OUs
            foreach (var currentOu in currentOus)
            {
                if (!organizationUnitIds.Contains(currentOu.Id))
                {
                    await RemoveFromOrganizationUnitAsync(user, currentOu);
                }
            }

            //Add to added OUs
            foreach (var organizationUnitId in organizationUnitIds)
            {
                if (currentOus.All(ou => ou.Id != organizationUnitId))
                {
                    await AddToOrganizationUnitAsync(
                        user,
                        await _organizationUnitRepository.GetAsync(organizationUnitId)
                        );
                }
            }
        }

        /// <summary>
        /// 获取组织架构
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual Task<List<OrganizationUnit>> GetOrganizationUnitsAsync(TUser user)
        {
            var query = from uou in _userOrganizationUnitRepository.GetAll()
                        join ou in _organizationUnitRepository.GetAll() on uou.OrganizationUnitId equals ou.Id
                        where uou.UserId == user.Id
                        select ou;

            return Task.FromResult(query.ToList());
        }

        /// <summary>
        /// 获取组织架构中的用户
        /// </summary>
        /// <param name="organizationUnit">组织</param>
        /// <param name="includeChildren">是否包含子节点</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual Task<List<TUser>> GetUsersInOrganizationUnit(OrganizationUnit organizationUnit, bool includeChildren = false)
        {
            if (!includeChildren)
            {
                var query = from uou in _userOrganizationUnitRepository.GetAll()
                            join user in AbpStore.Users on uou.UserId equals user.Id
                            where uou.OrganizationUnitId == organizationUnit.Id
                            select user;

                return Task.FromResult(query.ToList());
            }
            else
            {
                var query = from uou in _userOrganizationUnitRepository.GetAll()
                            join user in AbpStore.Users on uou.UserId equals user.Id
                            join ou in _organizationUnitRepository.GetAll() on uou.OrganizationUnitId equals ou.Id
                            where ou.Code.StartsWith(organizationUnit.Code)
                            select user;

                return Task.FromResult(query.ToList());
            }
        }
        /// <summary>
        /// 注册两个因子提供者 ？？
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        public virtual void RegisterTwoFactorProviders(int? tenantId)
        {
            TwoFactorProviders.Clear();

            if (!IsTrue(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled, tenantId))
            {
                return;
            }

            if (EmailService != null &&
                IsTrue(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEmailProviderEnabled, tenantId))
            {
                RegisterTwoFactorProvider(
                    L("Email"),
                    new EmailTokenProvider<TUser, long>
                    {
                        Subject = L("EmailSecurityCodeSubject"),
                        BodyFormat = L("EmailSecurityCodeBody")
                    }
                );
            }

            if (SmsService != null &&
                IsTrue(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsSmsProviderEnabled, tenantId))
            {
                RegisterTwoFactorProvider(
                    L("Sms"),
                    new PhoneNumberTokenProvider<TUser, long>
                    {
                        MessageFormat = L("SmsSecurityCodeMessage")
                    }
                );
            }
        }
        /// <summary>
        /// 初始化锁定设置
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        public virtual void InitializeLockoutSettings(int? tenantId)
        {
            UserLockoutEnabledByDefault = IsTrue(AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled, tenantId);
            DefaultAccountLockoutTimeSpan = TimeSpan.FromSeconds(GetSettingValue<int>(AbpZeroSettingNames.UserManagement.UserLockOut.DefaultAccountLockoutSeconds, tenantId));
            MaxFailedAccessAttemptsBeforeLockout = GetSettingValue<int>(AbpZeroSettingNames.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout, tenantId);
        }
        /// <summary>
        /// 获得有效的双因素提供者
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public override async Task<IList<string>> GetValidTwoFactorProvidersAsync(long userId)
        {
            var user = await GetUserByIdAsync(userId);

            RegisterTwoFactorProviders(user.TenantId);

            return await base.GetValidTwoFactorProvidersAsync(userId);
        }
        /// <summary>
        /// 通知双因素令牌
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="twoFactorProvider">双因素提供者</param>
        /// <param name="token">令牌字符串</param>
        /// <returns></returns>
        public override async Task<IdentityResult> NotifyTwoFactorTokenAsync(long userId, string twoFactorProvider, string token)
        {
            var user = await GetUserByIdAsync(userId);

            RegisterTwoFactorProviders(user.TenantId);

            return await base.NotifyTwoFactorTokenAsync(userId, twoFactorProvider, token);
        }
        /// <summary>
        /// 生成双因素令牌
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="twoFactorProvider">双因素提供者</param>
        /// <returns></returns>
        public override async Task<string> GenerateTwoFactorTokenAsync(long userId, string twoFactorProvider)
        {
            var user = await GetUserByIdAsync(userId);

            RegisterTwoFactorProviders(user.TenantId);

            return await base.GenerateTwoFactorTokenAsync(userId, twoFactorProvider);
        }
        /// <summary>
        /// 验证双因素令牌
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="twoFactorProvider">双因素提供者</param>
        /// <param name="token">令牌</param>
        /// <returns></returns>
        public override async Task<bool> VerifyTwoFactorTokenAsync(long userId, string twoFactorProvider, string token)
        {
            var user = await GetUserByIdAsync(userId);

            RegisterTwoFactorProviders(user.TenantId);

            return await base.VerifyTwoFactorTokenAsync(userId, twoFactorProvider, token);
        }
        /// <summary>
        /// 获取用户权限缓存项
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        private async Task<UserPermissionCacheItem> GetUserPermissionCacheItemAsync(long userId)
        {
            var cacheKey = userId + "@" + (GetCurrentTenantId() ?? 0);
            return await _cacheManager.GetUserPermissionCache().GetAsync(cacheKey, async () =>
            {
                var newCacheItem = new UserPermissionCacheItem(userId);

                foreach (var roleName in await GetRolesAsync(userId))
                {
                    newCacheItem.RoleIds.Add((await RoleManager.GetRoleByNameAsync(roleName)).Id);
                }

                foreach (var permissionInfo in await UserPermissionStore.GetPermissionsAsync(userId))
                {
                    if (permissionInfo.IsGranted)
                    {
                        newCacheItem.GrantedPermissions.Add(permissionInfo.Name);
                    }
                    else
                    {
                        newCacheItem.ProhibitedPermissions.Add(permissionInfo.Name);
                    }
                }

                return newCacheItem;
            });
        }
        /// <summary>
        /// Is True
        /// </summary>
        /// <param name="settingName">设置名称</param>
        /// <param name="tenantId">商户ID</param>
        /// <returns></returns>
        private bool IsTrue(string settingName, int? tenantId)
        {
            return GetSettingValue<bool>(settingName, tenantId);
        }
        /// <summary>
        /// 获取设置的值
        /// </summary>
        /// <typeparam name="T">设置对象</typeparam>
        /// <param name="settingName">设置名称</param>
        /// <param name="tenantId">商户ID</param>
        /// <returns></returns>
        private T GetSettingValue<T>(string settingName, int? tenantId) where T : struct
        {
            return tenantId == null
                ? _settingManager.GetSettingValueForApplication<T>(settingName)
                : _settingManager.GetSettingValueForTenant<T>(settingName, tenantId.Value);
        }
        /// <summary>
        /// 获取本地化字符串
        /// </summary>
        /// <param name="name">获取字符串的Key</param>
        /// <returns></returns>
        private string L(string name)
        {
            return LocalizationManager.GetString(AbpZeroConsts.LocalizationSourceName, name);
        }
        /// <summary>
        /// 获取当前商户ID
        /// </summary>
        /// <returns></returns>
        private int? GetCurrentTenantId()
        {
            if (_unitOfWorkManager.Current != null)
            {
                return _unitOfWorkManager.Current.GetTenantId();
            }

            return AbpSession.TenantId;
        }
    }
}