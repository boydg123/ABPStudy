using Abp.Authorization;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Runtime.Session;
using Hangfire.Dashboard;

namespace Abp.Hangfire
{
    /// <summary>
    /// Abp Hangfire 授权过滤器
    /// </summary>
    public class AbpHangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        /// <summary>
        /// IOC解析器
        /// </summary>
        public IIocResolver IocResolver { get; set; }

        /// <summary>
        /// 所需的权限名称
        /// </summary>
        private readonly string _requiredPermissionName;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="requiredPermissionName">所需的权限名称</param>
        public AbpHangfireAuthorizationFilter(string requiredPermissionName = null)
        {
            _requiredPermissionName = requiredPermissionName;

            IocResolver = IocManager.Instance;
        }

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="context">Dashboard上下文</param>
        /// <returns></returns>
        public bool Authorize(DashboardContext context)
        {
            if (!IsLoggedIn())
            {
                return false;
            }

            if (!_requiredPermissionName.IsNullOrEmpty() && !IsPermissionGranted(_requiredPermissionName))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 是否登录
        /// </summary>
        /// <returns></returns>
        private bool IsLoggedIn()
        {
            using (var abpSession = IocResolver.ResolveAsDisposable<IAbpSession>())
            {
                return abpSession.Object.UserId.HasValue;
            }
        }

        /// <summary>
        /// 是否许可
        /// </summary>
        /// <param name="requiredPermissionName"></param>
        /// <returns></returns>
        private bool IsPermissionGranted(string requiredPermissionName)
        {
            using (var permissionChecker = IocResolver.ResolveAsDisposable<IPermissionChecker>())
            {
                return permissionChecker.Object.IsGranted(requiredPermissionName);
            }
        }
    }
}
