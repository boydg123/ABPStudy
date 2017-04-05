using System.Threading.Tasks;

namespace Abp.Application.Features
{
    /// <summary>
    /// Most simple implementation of <see cref="IFeatureDependency"/>.It checks one or more features if they are enabled.
    /// <see cref="IFeatureDependency"/>最简单的实现，它将检查一个或多个可用的功能
    /// </summary>
    public class SimpleFeatureDependency : IFeatureDependency
    {
        /// <summary>
        /// A list of features to be checked if they are enabled.
        /// 将要被检查的可用功能列表
        /// </summary>
        public string[] Features { get; set; }

        /// <summary>
        /// If this property is set to true, all of the <see cref="Features"/> must be enabled.
        /// 如果此属性设置为true，所有的功能必须可用
        /// If it's false, at least one of the <see cref="Features"/> must be enabled.
        /// 如果设置为false.至少一个功能必须可用
        /// Default: false.
        /// </summary>
        public bool RequiresAll { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleFeatureDependency"/> class.
        /// 构造函数
        /// </summary>
        /// <param name="features">The features. / 功能</param>
        public SimpleFeatureDependency(params string[] features)
        {
            Features = features;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleFeatureDependency"/> class.
        /// </summary>
        /// <param name="requiresAll">
        /// If this is set to true, all of the <see cref="Features"/> must be enabled.
        /// If it's false, at least one of the <see cref="Features"/> must be enabled.
        /// </param>
        /// <param name="features">The features.</param>
        public SimpleFeatureDependency(bool requiresAll, params string[] features)
            : this(features)
        {
            RequiresAll = requiresAll;
        }

        /// <inheritdoc/>
        public Task<bool> IsSatisfiedAsync(IFeatureDependencyContext context)
        {
            return context.TenantId.HasValue
                ? context.FeatureChecker.IsEnabledAsync(context.TenantId.Value, RequiresAll, Features)
                : context.FeatureChecker.IsEnabledAsync(RequiresAll, Features);
        }
    }
}