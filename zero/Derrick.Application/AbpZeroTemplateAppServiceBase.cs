using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Microsoft.AspNet.Identity;
using Derrick.Authorization.Users;
using Derrick.MultiTenancy;

namespace Derrick
{
    /// <summary>
    /// All application services in this application is derived from this class.We can add common application service methods here.
    /// 应用程序服务基类，可以在此类添加一些公用方法
    /// </summary>
    public abstract class AbpZeroTemplateAppServiceBase : ApplicationService
    {
        /// <summary>
        /// 商户管理器
        /// </summary>
        public TenantManager TenantManager { get; set; }
        /// <summary>
        /// 用户管理器
        /// </summary>
        public UserManager UserManager { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        protected AbpZeroTemplateAppServiceBase()
        {
            LocalizationSourceName = AbpZeroTemplateConsts.LocalizationSourceName;
        }
        /// <summary>
        /// 获取当前用户 - 异步
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId());
            if (user == null)
            {
                throw new ApplicationException("There is no current user!");
            }

            return user;
        }
        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        protected virtual User GetCurrentUser()
        {
            var user = UserManager.FindById(AbpSession.GetUserId());
            if (user == null)
            {
                throw new ApplicationException("There is no current user!");
            }

            return user;
        }
        /// <summary>
        /// 获取当前商户 - 异步
        /// </summary>
        /// <returns></returns>
        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
            }
        }
        /// <summary>
        /// 获取当前商户
        /// </summary>
        /// <returns></returns>
        protected virtual Tenant GetCurrentTenant()
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                return TenantManager.GetById(AbpSession.GetTenantId());
            }
        }
        /// <summary>
        /// 检查错误
        /// </summary>
        /// <param name="identityResult">标识结果</param>
        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}