using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Auditing;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.MultiTenancy;
using Abp.Timing;
using Abp.Zero.Configuration;
using Microsoft.AspNet.Identity;

namespace Abp.Authorization
{
    //SignInManager<TUser, long>,
    /// <summary>
    /// ABP Login管理
    /// </summary>
    /// <typeparam name="TTenant">商户对象</typeparam>
    /// <typeparam name="TRole">角色对象</typeparam>
    /// <typeparam name="TUser">用户对象</typeparam>
    public class AbpLogInManager<TTenant, TRole, TUser> : ITransientDependency
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser>, new()
        where TUser : AbpUser<TUser>
    {
        /// <summary>
        /// 客户端信息提供者
        /// </summary>
        public IClientInfoProvider ClientInfoProvider { get; set; }
        /// <summary>
        /// 多租户配置
        /// </summary>
        protected IMultiTenancyConfig MultiTenancyConfig { get; }
        /// <summary>
        /// 商户仓储
        /// </summary>
        protected IRepository<TTenant> TenantRepository { get; }
        /// <summary>
        /// 工作单元
        /// </summary>
        protected IUnitOfWorkManager UnitOfWorkManager { get; }
        /// <summary>
        /// ABP 用户管理
        /// </summary>
        protected AbpUserManager<TRole, TUser> UserManager { get; }
        /// <summary>
        /// 设置管理
        /// </summary>
        protected ISettingManager SettingManager { get; }
        /// <summary>
        /// 用户的登录信息仓储
        /// </summary>
        protected IRepository<UserLoginAttempt, long> UserLoginAttemptRepository { get; }
        /// <summary>
        /// 用户管理配置
        /// </summary>
        protected IUserManagementConfig UserManagementConfig { get; }
        /// <summary>
        /// IOC解析器
        /// </summary>
        protected IIocResolver IocResolver { get; }
        /// <summary>
        /// ABP角色管理器
        /// </summary>
        protected AbpRoleManager<TRole, TUser> RoleManager { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userManager">ABP 用户管理</param>
        /// <param name="multiTenancyConfig">多租户配置</param>
        /// <param name="tenantRepository">商户仓储</param>
        /// <param name="unitOfWorkManager">工作单元</param>
        /// <param name="settingManager">设置管理</param>
        /// <param name="userLoginAttemptRepository">用户的登录信息仓储</param>
        /// <param name="userManagementConfig">用户管理配置</param>
        /// <param name="iocResolver">IOC解析器</param>
        /// <param name="roleManager">ABP角色管理器</param>
        public AbpLogInManager(
            AbpUserManager<TRole, TUser> userManager,
            IMultiTenancyConfig multiTenancyConfig,
            IRepository<TTenant> tenantRepository,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            IRepository<UserLoginAttempt, long> userLoginAttemptRepository,
            IUserManagementConfig userManagementConfig,
            IIocResolver iocResolver, 
            AbpRoleManager<TRole, TUser> roleManager)
        {
            MultiTenancyConfig = multiTenancyConfig;
            TenantRepository = tenantRepository;
            UnitOfWorkManager = unitOfWorkManager;
            SettingManager = settingManager;
            UserLoginAttemptRepository = userLoginAttemptRepository;
            UserManagementConfig = userManagementConfig;
            IocResolver = iocResolver;
            RoleManager = roleManager;
            UserManager = userManager;

            ClientInfoProvider = NullClientInfoProvider.Instance;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="login">用户登录信息</param>
        /// <param name="tenancyName">租户名称</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<AbpLoginResult<TTenant, TUser>> LoginAsync(UserLoginInfo login, string tenancyName = null)
        {
            var result = await LoginAsyncInternal(login, tenancyName);
            await SaveLoginAttempt(result, tenancyName, login.ProviderKey + "@" + login.LoginProvider);
            return result;
        }

        /// <summary>
        /// 内部登录 - 异步
        /// </summary>
        /// <param name="login">用户登录信息</param>
        /// <param name="tenancyName">租户名称</param>
        /// <returns></returns>
        private async Task<AbpLoginResult<TTenant, TUser>> LoginAsyncInternal(UserLoginInfo login, string tenancyName)
        {
            if (login == null || login.LoginProvider.IsNullOrEmpty() || login.ProviderKey.IsNullOrEmpty())
            {
                throw new ArgumentException("login");
            }

            //Get and check tenant
            TTenant tenant = null;
            if (!MultiTenancyConfig.IsEnabled)
            {
                tenant = await GetDefaultTenantAsync();
            }
            else if (!string.IsNullOrWhiteSpace(tenancyName))
            {
                tenant = await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
                if (tenant == null)
                {
                    return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidTenancyName);
                }

                if (!tenant.IsActive)
                {
                    return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.TenantIsNotActive, tenant);
                }
            }

            int? tenantId = tenant == null ? (int?)null : tenant.Id;
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var user = await UserManager.AbpStore.FindAsync(tenantId, login);
                if (user == null)
                {
                    return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.UnknownExternalLogin, tenant);
                }

                return await CreateLoginResultAsync(user, tenant);
            }
        }

        /// <summary>
        /// 异步登录
        /// </summary>
        /// <param name="userNameOrEmailAddress">用户名或邮箱地址</param>
        /// <param name="plainPassword">密码</param>
        /// <param name="tenancyName">商户名称</param>
        /// <param name="shouldLockout">是否锁住</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<AbpLoginResult<TTenant, TUser>> LoginAsync(string userNameOrEmailAddress, string plainPassword, string tenancyName = null, bool shouldLockout = true)
        {
            var result = await LoginAsyncInternal(userNameOrEmailAddress, plainPassword, tenancyName, shouldLockout);
            await SaveLoginAttempt(result, tenancyName, userNameOrEmailAddress);
            return result;
        }

        /// <summary>
        /// 内部登录 - 异步
        /// </summary>
        /// <param name="userNameOrEmailAddress">用户名或邮箱地址</param>
        /// <param name="plainPassword">密码</param>
        /// <param name="tenancyName">商户名称</param>
        /// <param name="shouldLockout">是否锁住</param>
        /// <returns></returns>
        private async Task<AbpLoginResult<TTenant, TUser>> LoginAsyncInternal(string userNameOrEmailAddress, string plainPassword, string tenancyName, bool shouldLockout)
        {
            if (userNameOrEmailAddress.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(userNameOrEmailAddress));
            }

            if (plainPassword.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(plainPassword));
            }

            //Get and check tenant
            TTenant tenant = null;
            using (UnitOfWorkManager.Current.SetTenantId(null))
            {
                if (!MultiTenancyConfig.IsEnabled)
                {
                    tenant = await GetDefaultTenantAsync();
                }
                else if (!string.IsNullOrWhiteSpace(tenancyName))
                {
                    tenant = await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
                    if (tenant == null)
                    {
                        return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidTenancyName);
                    }

                    if (!tenant.IsActive)
                    {
                        return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.TenantIsNotActive, tenant);
                    }
                }
            }

            var tenantId = tenant == null ? (int?)null : tenant.Id;
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                //TryLoginFromExternalAuthenticationSources method may create the user, that's why we are calling it before AbpStore.FindByNameOrEmailAsync
                var loggedInFromExternalSource = await TryLoginFromExternalAuthenticationSources(userNameOrEmailAddress, plainPassword, tenant);

                var user = await UserManager.AbpStore.FindByNameOrEmailAsync(tenantId, userNameOrEmailAddress);
                if (user == null)
                {
                    return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidUserNameOrEmailAddress, tenant);
                }

                if (!loggedInFromExternalSource)
                {
                    UserManager.InitializeLockoutSettings(tenantId);

                    if (await UserManager.IsLockedOutAsync(user.Id))
                    {
                        return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.LockedOut, tenant, user);
                    }

                    var verificationResult = UserManager.PasswordHasher.VerifyHashedPassword(user.Password, plainPassword);
                    if (verificationResult != PasswordVerificationResult.Success)
                    {
                        if (shouldLockout)
                        {
                            if (await TryLockOutAsync(tenantId, user.Id))
                            {
                                return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.LockedOut, tenant, user);
                            }
                        }

                        return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.InvalidPassword, tenant, user);
                    }

                    await UserManager.ResetAccessFailedCountAsync(user.Id);
                }

                return await CreateLoginResultAsync(user, tenant);
            }
        }

        /// <summary>
        /// 创建登录结果
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="tenant">商户</param>
        /// <returns></returns>
        private async Task<AbpLoginResult<TTenant, TUser>> CreateLoginResultAsync(TUser user, TTenant tenant = null)
        {
            if (!user.IsActive)
            {
                return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.UserIsNotActive);
            }

            if (await IsEmailConfirmationRequiredForLoginAsync(user.TenantId) && !user.IsEmailConfirmed)
            {
                return new AbpLoginResult<TTenant, TUser>(AbpLoginResultType.UserEmailIsNotConfirmed);
            }

            user.LastLoginTime = Clock.Now;

            await UserManager.AbpStore.UpdateAsync(user);

            await UnitOfWorkManager.Current.SaveChangesAsync();

            return new AbpLoginResult<TTenant, TUser>(
                tenant,
                user,
                await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie)
            );
        }
        /// <summary>
        /// 保存登录尝试
        /// </summary>
        /// <param name="loginResult">登录结果</param>
        /// <param name="tenancyName">商户名称</param>
        /// <param name="userNameOrEmailAddress">用户名或邮箱</param>
        /// <returns></returns>
        private async Task SaveLoginAttempt(AbpLoginResult<TTenant, TUser> loginResult, string tenancyName, string userNameOrEmailAddress)
        {
            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                var tenantId = loginResult.Tenant != null ? loginResult.Tenant.Id : (int?)null;
                using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    var loginAttempt = new UserLoginAttempt
                    {
                        TenantId = tenantId,
                        TenancyName = tenancyName,

                        UserId = loginResult.User != null ? loginResult.User.Id : (long?) null,
                        UserNameOrEmailAddress = userNameOrEmailAddress,

                        Result = loginResult.Result,

                        BrowserInfo = ClientInfoProvider.BrowserInfo,
                        ClientIpAddress = ClientInfoProvider.ClientIpAddress,
                        ClientName = ClientInfoProvider.ComputerName,
                    };

                    await UserLoginAttemptRepository.InsertAsync(loginAttempt);
                    await UnitOfWorkManager.Current.SaveChangesAsync();

                    await uow.CompleteAsync();
                }
            }
        }

        /// <summary>
        /// 尝试锁定
        /// </summary>
        /// <param name="tenantId">租户ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        private async Task<bool> TryLockOutAsync(int? tenantId, long userId)
        {
            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    (await UserManager.AccessFailedAsync(userId)).CheckErrors();

                    var isLockOut =  await UserManager.IsLockedOutAsync(userId);

                    await UnitOfWorkManager.Current.SaveChangesAsync();

                    await uow.CompleteAsync();

                    return isLockOut;
                }
            }
        }

        /// <summary>
        /// 尝试从外部身份验证源登录
        /// </summary>
        /// <param name="userNameOrEmailAddress">用户名或邮箱</param>
        /// <param name="plainPassword">密码</param>
        /// <param name="tenant">商户信息</param>
        /// <returns></returns>
        private async Task<bool> TryLoginFromExternalAuthenticationSources(string userNameOrEmailAddress, string plainPassword, TTenant tenant)
        {
            if (!UserManagementConfig.ExternalAuthenticationSources.Any())
            {
                return false;
            }

            foreach (var sourceType in UserManagementConfig.ExternalAuthenticationSources)
            {
                using (var source = IocResolver.ResolveAsDisposable<IExternalAuthenticationSource<TTenant, TUser>>(sourceType))
                {
                    if (await source.Object.TryAuthenticateAsync(userNameOrEmailAddress, plainPassword, tenant))
                    {
                        var tenantId = tenant == null ? (int?)null : tenant.Id;
                        using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                        {
                            var user = await UserManager.AbpStore.FindByNameOrEmailAsync(tenantId, userNameOrEmailAddress);
                            if (user == null)
                            {
                                user = await source.Object.CreateUserAsync(userNameOrEmailAddress, tenant);

                                user.TenantId = tenantId;
                                user.AuthenticationSource = source.Object.Name;
                                user.Password = UserManager.PasswordHasher.HashPassword(Guid.NewGuid().ToString("N").Left(16)); //Setting a random password since it will not be used

                                user.Roles = new List<UserRole>();
                                foreach (var defaultRole in RoleManager.Roles.Where(r => r.TenantId == tenantId && r.IsDefault).ToList())
                                {
                                    user.Roles.Add(new UserRole(tenantId, user.Id, defaultRole.Id));
                                }

                                await UserManager.AbpStore.CreateAsync(user);
                            }
                            else
                            {
                                await source.Object.UpdateUserAsync(user, tenant);

                                user.AuthenticationSource = source.Object.Name;

                                await UserManager.AbpStore.UpdateAsync(user);
                            }

                            await UnitOfWorkManager.Current.SaveChangesAsync();

                            return true;
                        }
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// 获取默认商户
        /// </summary>
        /// <returns></returns>
        private async Task<TTenant> GetDefaultTenantAsync()
        {
            var tenant = await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == AbpTenant<TUser>.DefaultTenantName);
            if (tenant == null)
            {
                throw new AbpException("There should be a 'Default' tenant if multi-tenancy is disabled!");
            }

            return tenant;
        }
        /// <summary>
        /// 电子邮件确认是否要异步登录
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <returns></returns>
        private async Task<bool> IsEmailConfirmationRequiredForLoginAsync(int? tenantId)
        {
            if (tenantId.HasValue)
            {
                return await SettingManager.GetSettingValueForTenantAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin, tenantId.Value);
            }

            return await SettingManager.GetSettingValueForApplicationAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);
        }
    }
}
