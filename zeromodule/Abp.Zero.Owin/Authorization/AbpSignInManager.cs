using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Abp.Zero.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Abp.Authorization
{
    /// <summary>
    /// ABP 登陆管理
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    public abstract class AbpSignInManager<TTenant, TRole, TUser> : SignInManager<TUser, long>, ITransientDependency
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser>, new()
        where TUser : AbpUser<TUser>
    {
        /// <summary>
        /// 设置管理
        /// </summary>
        private readonly ISettingManager _settingManager;
        /// <summary>
        /// 工作单元管理
        /// </summary>
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userManager">用户管理</param>
        /// <param name="authenticationManager">认证管理</param>
        /// <param name="settingManager">设置管理</param>
        /// <param name="unitOfWorkManager">工作单元管理</param>
        protected AbpSignInManager(
            AbpUserManager<TRole, TUser> userManager,
            IAuthenticationManager authenticationManager,
            ISettingManager settingManager,
            IUnitOfWorkManager unitOfWorkManager)
            : base(
                  userManager,
                  authenticationManager)
        {
            _settingManager = settingManager;
            _unitOfWorkManager = unitOfWorkManager;
        }

        /// <summary>
        /// This method can return two results:
        /// 这个方法可以返回两个结果
        /// <see cref="SignInStatus.Success"/> indicates that user has successfully signed in. / 指示用户已成功登录
        /// <see cref="SignInStatus.RequiresVerification"/> indicates that user has successfully signed in. / 用户需要验证
        /// </summary>
        /// <param name="loginResult">The login result received from <see cref="AbpLogInManager{TTenant,TRole,TUser}"/> Should be Success. / 从<see cref="AbpLogInManager{TTenant,TRole,TUser}"/>接收登录结果应该是成功的</param>
        /// <param name="isPersistent">True to use persistent cookie. / 是否使用持久Cooike</param>
        /// <param name="rememberBrowser">Remember user's browser (and not use two factor auth again) or not. / 是否记住用户的浏览器(而不是又使用双因素认证)</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">loginResult.Result should be success in order to sign in! / 登录结果，结果应该是成功的标志</exception>
        [UnitOfWork]
        public virtual async Task<SignInStatus> SignInOrTwoFactor(AbpLoginResult<TTenant, TUser> loginResult, bool isPersistent, bool? rememberBrowser = null)
        {
            if (loginResult.Result != AbpLoginResultType.Success)
            {
                throw new ArgumentException("loginResult.Result should be success in order to sign in!");
            }

            using (_unitOfWorkManager.Current.SetTenantId(loginResult.Tenant?.Id))
            {
                if (IsTrue(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled, loginResult.Tenant?.Id))
                {
                    UserManager.As<AbpUserManager<TRole, TUser>>().RegisterTwoFactorProviders(loginResult.Tenant?.Id);

                    if (await UserManager.GetTwoFactorEnabledAsync(loginResult.User.Id))
                    {
                        if ((await UserManager.GetValidTwoFactorProvidersAsync(loginResult.User.Id)).Count > 0)
                        {
                            if (!await AuthenticationManager.TwoFactorBrowserRememberedAsync(loginResult.User.Id.ToString()) || 
                                rememberBrowser == false)
                            {
                                var claimsIdentity = new ClaimsIdentity(DefaultAuthenticationTypes.TwoFactorCookie);

                                claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, loginResult.User.Id.ToString()));

                                if (loginResult.Tenant != null)
                                {
                                    claimsIdentity.AddClaim(new Claim(AbpClaimTypes.TenantId, loginResult.Tenant.Id.ToString()));
                                }

                                AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, claimsIdentity);
                                return SignInStatus.RequiresVerification;
                            }
                        }
                    }
                }

                SignIn(loginResult, isPersistent, rememberBrowser);
                return SignInStatus.Success;
            }
        }

        /// <param name="loginResult">The login result received from <see cref="AbpLogInManager{TTenant,TRole,TUser}"/> Should be Success. / 从<see cref="AbpLogInManager{TTenant,TRole,TUser}"/>接收登录结果应该是成功的</param>
        /// <param name="isPersistent">True to use persistent cookie. / 是否使用持久Cooike</param>
        /// <param name="rememberBrowser">Remember user's browser (and not use two factor auth again) or not. / 是否记住用户的浏览器(而不是又使用双因素认证)</param>
        [UnitOfWork]
        public virtual void SignIn(AbpLoginResult<TTenant, TUser> loginResult, bool isPersistent, bool? rememberBrowser = null)
        {
            if (loginResult.Result != AbpLoginResultType.Success)
            {
                throw new ArgumentException("loginResult.Result should be success in order to sign in!");
            }

            using (_unitOfWorkManager.Current.SetTenantId(loginResult.Tenant?.Id))
            {
                AuthenticationManager.SignOut(
                    DefaultAuthenticationTypes.ExternalCookie,
                    DefaultAuthenticationTypes.TwoFactorCookie
                );

                if (rememberBrowser == null)
                {
                    rememberBrowser = IsTrue(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled, loginResult.Tenant?.Id);
                }

                if (rememberBrowser == true)
                {
                    var rememberBrowserIdentity = AuthenticationManager.CreateTwoFactorRememberBrowserIdentity(loginResult.User.Id.ToString());
                    AuthenticationManager.SignIn(
                        new AuthenticationProperties
                        {
                            IsPersistent = isPersistent
                        },
                        loginResult.Identity,
                        rememberBrowserIdentity
                    );
                }
                else
                {
                    AuthenticationManager.SignIn(
                        new AuthenticationProperties
                        {
                            IsPersistent = isPersistent
                        },
                        loginResult.Identity
                    );
                }
            }
        }
        /// <summary>
        /// 获取已验证的商户ID
        /// </summary>
        /// <returns></returns>
        public virtual async Task<int?> GetVerifiedTenantIdAsync()
        {
            var authenticateResult = await AuthenticationManager.AuthenticateAsync(
                DefaultAuthenticationTypes.TwoFactorCookie
            );

            return authenticateResult?.Identity?.GetTenantId();
        }
        /// <summary>
        /// 是否设置
        /// </summary>
        /// <param name="settingName">设置名称</param>
        /// <param name="tenantId">商户ID</param>
        /// <returns></returns>
        private bool IsTrue(string settingName, int? tenantId)
        {
            return tenantId == null
                ? _settingManager.GetSettingValueForApplication<bool>(settingName)
                : _settingManager.GetSettingValueForTenant<bool>(settingName, tenantId.Value);
        }
    }
}
