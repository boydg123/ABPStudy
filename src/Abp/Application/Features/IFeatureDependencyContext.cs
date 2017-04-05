using Abp.Dependency;

namespace Abp.Application.Features
{
    /// <summary>
    /// Used in <see cref="IFeatureDependency.IsSatisfiedAsync"/> method.
    /// 在<see cref="IFeatureDependency.IsSatisfiedAsync"/>方法中使用
    /// </summary>
    public interface IFeatureDependencyContext
    {
        /// <summary>
        /// Tenant id which required the feature.Null for current tenant.
        /// 需要此功能的租户ID，NULL或者当前租户
        /// </summary>
        int? TenantId { get; }

        /// <summary>
        /// Gets the <see cref="IIocResolver"/>.
        /// IOC解析器
        /// </summary>
        /// <value>
        /// The ioc resolver.
        /// </value>
        IIocResolver IocResolver { get; }

        /// <summary>
        /// Gets the <see cref="IFeatureChecker"/>.
        /// 功能检查者
        /// </summary>
        /// <value>
        /// The feature checker.
        /// </value>
        IFeatureChecker FeatureChecker { get; }
    }
}