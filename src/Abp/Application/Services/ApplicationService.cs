using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Features;
using Abp.Authorization;
using Abp.Runtime.Session;

namespace Abp.Application.Services
{
    /// <summary>
    /// This class can be used as a base class for application services. 
    /// 此类能被用作服务的基类
    /// </summary>
    public abstract class ApplicationService : AbpServiceBase, IApplicationService, IAvoidDuplicateCrossCuttingConcerns
    {
        public static string[] CommonPostfixes = { "AppService", "ApplicationService" };

        /// <summary>
        /// Gets current session information.
        /// 获取当前会话信息
        /// </summary>
        public IAbpSession AbpSession { get; set; }

        /// <summary>
        /// Reference to the permission manager.
        /// 权限管理器引用
        /// </summary>
        public IPermissionManager PermissionManager { protected get; set; }

        /// <summary>
        /// Reference to the permission checker.
        /// 权限检查器引用
        /// </summary>
        public IPermissionChecker PermissionChecker { protected get; set; }

        /// <summary>
        /// Reference to the feature manager.
        /// 功能管理器的引用
        /// </summary>
        public IFeatureManager FeatureManager { protected get; set; }

        /// <summary>
        /// Reference to the feature checker.
        /// 功能检查器的引用
        /// </summary>
        public IFeatureChecker FeatureChecker { protected get; set; }

        /// <summary>
        /// Gets the applied cross cutting concerns.
        /// 得到应用横切关注点
        /// </summary>
        public List<string> AppliedCrossCuttingConcerns { get; } = new List<string>();

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        protected ApplicationService()
        {
            AbpSession = NullAbpSession.Instance;
            PermissionChecker = NullPermissionChecker.Instance;
        }

        /// <summary>
        /// Checks if current user is granted for a permission.
        /// 异步检查当前用户是否被授予一个权限
        /// </summary>
        /// <param name="permissionName">Name of the permission / 权限名称</param>
        protected virtual Task<bool> IsGrantedAsync(string permissionName)
        {
            return PermissionChecker.IsGrantedAsync(permissionName);
        }

        /// <summary>
        /// Checks if current user is granted for a permission.
        /// 检查当前用户是否被授予一个权限
        /// </summary>
        /// <param name="permissionName">Name of the permission / 权限名称</param>
        protected virtual bool IsGranted(string permissionName)
        {
            return PermissionChecker.IsGranted(permissionName);
        }

        /// <summary>
        /// Checks if given feature is enabled for current tenant.
        /// 异步检查当前租户给定的功能是否可用
        /// </summary>
        /// <param name="featureName">Name of the feature / 功能的名称</param>
        /// <returns></returns>
        protected virtual Task<bool> IsEnabledAsync(string featureName)
        {
            return FeatureChecker.IsEnabledAsync(featureName);
        }

        /// <summary>
        /// Checks if given feature is enabled for current tenant.
        /// 检查当前租户给定的功能是否可用
        /// </summary>
        /// <param name="featureName">Name of the feature / 功能的名称</param>
        /// <returns></returns>
        protected virtual bool IsEnabled(string featureName)
        {
            return FeatureChecker.IsEnabled(featureName);
        }
    }
}
