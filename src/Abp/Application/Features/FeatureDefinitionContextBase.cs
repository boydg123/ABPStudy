using Abp.Collections.Extensions;
using Abp.Localization;
using Abp.UI.Inputs;

namespace Abp.Application.Features
{
    /// <summary>
    /// Base for implementing <see cref="IFeatureDefinitionContext"/>.
    /// <see cref="IFeatureDefinitionContext"/>的基本实现
    /// </summary>
    public abstract class FeatureDefinitionContextBase : IFeatureDefinitionContext
    {
        /// <summary>
        /// 功能字典
        /// </summary>
        protected readonly FeatureDictionary Features;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureDefinitionContextBase"/> class.
        /// 构造函数
        /// </summary>
        protected FeatureDefinitionContextBase()
        {
            Features = new FeatureDictionary();
        }

        /// <summary>
        /// Creates a new feature.
        /// 创建一个新功能
        /// </summary>
        /// <param name="name">Unique name of the feature / 功能的唯一名称</param>
        /// <param name="defaultValue">Default value / 默认值</param>
        /// <param name="displayName">Display name of the feature / 功能的显示名称</param>
        /// <param name="description">A brief description for this feature / 功能的简要描述</param>
        /// <param name="scope">Feature scope / 功能域</param>
        /// <param name="inputType">Input type / 输入类型</param>
        public Feature Create(string name, string defaultValue, ILocalizableString displayName = null,
            ILocalizableString description = null, FeatureScopes scope = FeatureScopes.All, IInputType inputType = null)
        {
            if (Features.ContainsKey(name))
            {
                throw new AbpException("There is already a feature with name: " + name);
            }

            var feature = new Feature(name, defaultValue, displayName, description, scope, inputType);
            Features[feature.Name] = feature;
            return feature;

        }

        /// <summary>
        /// Gets a feature with given name or null if can not find.
        /// 根据给定名称获取功能，如果没找到则返回NULL
        /// </summary>
        /// <param name="name">Unique name of the feature / 功能的唯一名称</param>
        /// <returns>
        /// <see cref="Feature" /> object or null / 对象或者null
        /// </returns>
        public Feature GetOrNull(string name)
        {
            return Features.GetOrDefault(name);
        }
    }
}