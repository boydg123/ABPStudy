using System;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.Authorization.Users;
using Abp.Web.Models;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Derrick.Authorization;
using Derrick.Authorization.Users;
using Derrick.MultiTenancy;
using Derrick.WebApi.Models;

namespace Derrick.WebApi.Controllers
{
    /// <summary>
    /// Account 控制器
    /// </summary>
    public class AccountController : AbpZeroTemplateApiControllerBase
    {
        /// <summary>
        /// 认证Bearer选项
        /// </summary>
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        /// <summary>
        /// 登录管理器
        /// </summary>
        private readonly LogInManager _logInManager;
        /// <summary>
        /// 登录结果帮助类
        /// </summary>
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static AccountController()
        {
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="abpLoginResultTypeHelper">登录结果帮助类</param>
        /// <param name="logInManager">登录管理</param>
        public AccountController(
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            LogInManager logInManager)
        {
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _logInManager = logInManager;
        }

        /// <summary>
        /// 认证
        /// </summary>
        /// <param name="loginModel">登录模型</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AjaxResponse> Authenticate(LoginModel loginModel)
        {
            var loginResult = await GetLoginResultAsync(
                loginModel.UsernameOrEmailAddress,
                loginModel.Password,
                loginModel.TenancyName
                );

            var ticket = new AuthenticationTicket(loginResult.Identity, new AuthenticationProperties());

            var currentUtc = new SystemClock().UtcNow;
            ticket.Properties.IssuedUtc = currentUtc;
            ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromMinutes(30));

            return new AjaxResponse(OAuthBearerOptions.AccessTokenFormat.Protect(ticket));
        }

        /// <summary>
        /// 获取登录结果 - 异步
        /// </summary>
        /// <param name="usernameOrEmailAddress">用户名或邮箱</param>
        /// <param name="password">密码</param>
        /// <param name="tenancyName">商户名</param>
        /// <returns></returns>
        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }
    }
}
