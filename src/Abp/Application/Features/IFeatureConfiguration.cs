using Abp.Collections;

namespace Abp.Application.Features
{
    /// <summary>
    /// Used to configure feature system.
    /// 用于配置功能系统
    /// </summary>
    public interface IFeatureConfiguration
    {
        /// <summary>
        /// Used to add/remove <see cref="FeatureProvider"/>s.
        /// 用于 添加/移除 <see cref="FeatureProvider"/>
        /// </summary>
        ITypeList<FeatureProvider> Providers { get; }
    }
}