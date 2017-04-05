using Abp.Localization;
using Abp.UI.Inputs;

namespace Abp.Application.Features
{
    /// <summary>
    /// Used in <see cref="FeatureProvider.SetFeatures"/> method as context.
    /// 在<see cref="FeatureProvider.SetFeatures"/>方法里用作上下文
    /// </summary>
    public interface IFeatureDefinitionContext
    {
        /// <summary>
        /// Creates a new feature.
        /// 创建一个新功能
        /// </summary>
        /// <param name="name">Unique name of the feature / 功能的唯一名称</param>
        /// <param name="defaultValue">Default value / 默认值</param>
        /// <param name="displayName">Display name of the feature / 功能的显示值</param>
        /// <param name="description">A brief description for this feature / 此功能的简要描述</param>
        /// <param name="scope">Feature scope / 功能范围</param>
        /// <param name="inputType">Input type / 输入类型</param>
        Feature Create(string name, string defaultValue, ILocalizableString displayName = null, ILocalizableString description = null, FeatureScopes scope = FeatureScopes.All, IInputType inputType = null);

        /// <summary>
        /// Gets a feature with given name or null if can not find.
        /// 根据给定名称获取功能，如果没有找到则返回NULL
        /// </summary>
        /// <param name="name">Unique name of the feature / 功能的唯一名称</param>
        /// <returns><see cref="Feature"/> object or null / 对象或者Null</returns>
        Feature GetOrNull(string name);
    }
}