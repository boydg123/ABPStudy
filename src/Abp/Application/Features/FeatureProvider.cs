using Abp.Dependency;

namespace Abp.Application.Features
{
    /// <summary>
    /// This class should be inherited in order to provide <see cref="Feature"/>s.
    /// 为了提供<see cref="Feature"/>，这个类应该被继承
    /// </summary>
    public abstract class FeatureProvider : ITransientDependency
    {
        /// <summary>
        /// Used to set <see cref="Feature"/>s.
        /// 用于设置<see cref="Feature"/>
        /// </summary>
        /// <param name="context">Feature definition context / 功能定义上下文</param>
        public abstract void SetFeatures(IFeatureDefinitionContext context);
    }
}