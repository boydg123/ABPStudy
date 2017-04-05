using Abp.Threading;

namespace Abp.Application.Features
{
    /// <summary>
    /// Extension methods for <see cref="IFeatureDependency"/>.
    /// <see cref="IFeatureDependency"/>����չ����
    /// </summary>
    public static class FeatureDependencyExtensions
    {
        /// <summary>
        /// Checks depended features and returns true if dependencies are satisfied.
        /// ��������Ĺ��ܺͷ���true�����������ϵ�õ�����
        /// </summary>
        /// <param name="featureDependency">The feature dependency. / ��������</param>
        /// <param name="context">The context. / ������</param>
        public static bool IsSatisfied(this IFeatureDependency featureDependency, IFeatureDependencyContext context)
        {
            return AsyncHelper.RunSync(() => featureDependency.IsSatisfiedAsync(context));
        }
    }
}