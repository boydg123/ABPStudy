using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Abp.Application.Features;
using Abp.Collections.Extensions;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime.Session;

namespace Abp.Authorization
{
    /// <summary>
    /// Permission manager.
    /// 权限管理器
    /// </summary>
    internal class PermissionManager : PermissionDefinitionContextBase, IPermissionManager, ISingletonDependency
    {
        /// <summary>
        /// ABP Session引用
        /// </summary>
        public IAbpSession AbpSession { get; set; }

        /// <summary>
        /// IOC管理器
        /// </summary>
        private readonly IIocManager _iocManager;

        /// <summary>
        /// 授权配置
        /// </summary>
        private readonly IAuthorizationConfiguration _authorizationConfiguration;

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        public PermissionManager(
            IIocManager iocManager,
            IAuthorizationConfiguration authorizationConfiguration)
        {
            _iocManager = iocManager;
            _authorizationConfiguration = authorizationConfiguration;

            AbpSession = NullAbpSession.Instance;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            foreach (var providerType in _authorizationConfiguration.Providers)
            {
                _iocManager.RegisterIfNot(providerType, DependencyLifeStyle.Transient); //TODO: Needed?
                using (var provider = _iocManager.ResolveAsDisposable<AuthorizationProvider>(providerType))
                {
                    provider.Object.SetPermissions(this);
                }
            }

            Permissions.AddAllPermissions();
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public Permission GetPermission(string name)
        {
            var permission = Permissions.GetOrDefault(name);
            if (permission == null)
            {
                throw new AbpException("There is no permission with name: " + name);
            }

            return permission;
        }

        /// <summary>
        /// 获取所有的权限
        /// </summary>
        /// <param name="tenancyFilter">租户过滤</param>
        /// <returns></returns>
        public IReadOnlyList<Permission> GetAllPermissions(bool tenancyFilter = true)
        {
            using (var featureDependencyContext = _iocManager.ResolveAsDisposable<FeatureDependencyContext>())
            {
                var featureDependencyContextObject = featureDependencyContext.Object;
                return Permissions.Values
                    .WhereIf(tenancyFilter, p => p.MultiTenancySides.HasFlag(AbpSession.MultiTenancySide))
                    .Where(p =>
                        p.FeatureDependency == null ||
                        AbpSession.MultiTenancySide == MultiTenancySides.Host ||
                        p.FeatureDependency.IsSatisfied(featureDependencyContextObject)
                    ).ToImmutableList();
            }
        }

        /// <summary>
        /// 获取所有的权限
        /// </summary>
        /// <param name="multiTenancySides">租户双方中的某一方</param>
        /// <returns></returns>
        public IReadOnlyList<Permission> GetAllPermissions(MultiTenancySides multiTenancySides)
        {
            using (var featureDependencyContext = _iocManager.ResolveAsDisposable<FeatureDependencyContext>())
            {
                var featureDependencyContextObject = featureDependencyContext.Object;
                return Permissions.Values
                    .Where(p => p.MultiTenancySides.HasFlag(multiTenancySides))
                    .Where(p =>
                        p.FeatureDependency == null ||
                        AbpSession.MultiTenancySide == MultiTenancySides.Host ||
                        (p.MultiTenancySides.HasFlag(MultiTenancySides.Host) &&
                         multiTenancySides.HasFlag(MultiTenancySides.Host)) ||
                        p.FeatureDependency.IsSatisfied(featureDependencyContextObject)
                    ).ToImmutableList();
            }
        }
    }
}