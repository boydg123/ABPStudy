using Abp.Threading;

namespace Abp.Application.Features
{
    /// <summary>
    /// Extension methods for <see cref="IFeatureDependency"/>.
    /// <see cref="IFeatureDependency"/>的扩展方法
    /// </summary>
    public static class FeatureDependencyExtensions
    {
        /// <summary>
        /// Checks depended features and returns true if dependencies are satisfied.
        /// 检查依赖的功能和返回true，如果依赖关系得到满足
        /// </summary>
        /// <param name="featureDependency">The feature dependency. / 功能依赖</param>
        /// <param name="context">The context. / 上下文</param>
        public static bool IsSatisfied(this IFeatureDependency featureDependency, IFeatureDependencyContext context)
        {
            return AsyncHelper.RunSync(() => featureDependency.IsSatisfiedAsync(context));
        }
    }
}