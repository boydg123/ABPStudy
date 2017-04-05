using Abp.Dependency;

namespace Abp.Application.Features
{
    /// <summary>
    /// Implementation of <see cref="IFeatureDependencyContext"/>.
    /// <see cref="IFeatureDependencyContext"/>的实现
    /// </summary>
    public class FeatureDependencyContext : IFeatureDependencyContext, ITransientDependency
    {
        /// <summary>
        /// 需要此功能的租户ID，NULL或者当前租户
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// IOC解析器
        /// </summary>
        public IIocResolver IocResolver { get; private set; }

        /// <summary>
        /// 功能检查者
        /// </summary>
        public IFeatureChecker FeatureChecker { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureDependencyContext"/> class.
        /// 构造函数
        /// </summary>
        public FeatureDependencyContext(IIocResolver iocResolver, IFeatureChecker featureChecker)
        {
            IocResolver = iocResolver;
            FeatureChecker = featureChecker;
        }
    }
}