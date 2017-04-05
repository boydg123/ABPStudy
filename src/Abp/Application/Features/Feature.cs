using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Abp.Collections.Extensions;
using Abp.Localization;
using Abp.UI.Inputs;

namespace Abp.Application.Features
{
    /// <summary>
    /// Defines a feature of the application. A <see cref="Feature"/> can be used in a multi-tenant application to enable disable some application features depending on editions and tenants.
    /// 定义应用程序的功能。可以在多租户应用程序中使用一个功能，以便根据版本和租户禁用某些应用程序功能。
    /// </summary>
    public class Feature
    {
        /// <summary>
        /// Gets/sets arbitrary objects related to this object.Gets null if given key does not exists.
        /// 获取/设置 此对象相关的任意对象,如果给定key不存在则获取的null
        /// This is a shortcut for <see cref="Attributes"/> dictionary.
        /// 这是一个<see cref="Attributes"/>字典的快捷方式
        /// </summary>
        /// <param name="key">Key / Key</param>
        public object this[string key]
        {
            get { return Attributes.GetOrDefault(key); }
            set { Attributes[key] = value; }
        }

        /// <summary>
        /// Arbitrary objects related to this object.These objects must be serializable.
        /// 此对象相关联的任意对象，那些对象必须可以被序列化
        /// </summary>
        public IDictionary<string, object> Attributes { get; private set; }

        /// <summary>
        /// Parent of this feature, if one exists.If set, this feature can be enabled only if parent is enabled.
        /// 此功能的父功能，如果存在。如果启用，此功能只能在父功能启用时启用
        /// </summary>
        public Feature Parent { get; private set; }

        /// <summary>
        /// Unique name of the feature.
        /// 功能的唯一名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Display name of the feature.This can be used to show features on UI.
        /// 功能的显示名称，可以在UI显示功能时使用
        /// </summary>
        public ILocalizableString DisplayName { get; set; }

        /// <summary>
        /// A brief description for this feature.This can be used to show feature description on UI. 
        /// 功能的简要描述，可以在UI显示功能描述时使用
        /// </summary>
        public ILocalizableString Description { get; set; }

        /// <summary>
        /// Input type.This can be used to prepare an input for changing this feature's value.
        /// 输入类型，这可用于准备输入更改此功能的值。
        /// Default: <see cref="CheckboxInputType"/>.
        /// </summary>
        public IInputType InputType { get; set; }

        /// <summary>
        /// Default value of the feature.This value is used if feature's value is not defined for current edition or tenant.
        /// 此功能的默认值，如果当前版本或租户没有定义功能值值，则使用此值。
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Feature's scope.
        /// 功能域
        /// </summary>
        public FeatureScopes Scope { get; set; }

        /// <summary>
        /// List of child features.
        /// 子功能列表
        /// </summary>
        public IReadOnlyList<Feature> Children
        {
            get { return _children.ToImmutableList(); }
        }
        private readonly List<Feature> _children;

        /// <summary>
        /// Creates a new feature.
        /// 创建一个新功能
        /// </summary>
        /// <param name="name">Unique name of the feature / 功能唯一名称</param>
        /// <param name="defaultValue">Default value / 默认值</param>
        /// <param name="displayName">Display name of the feature / 功能的显示名称</param>
        /// <param name="description">A brief description for this feature / 功能的简要描述</param>
        /// <param name="scope">Feature scope / 功能域</param>
        /// <param name="inputType">Input type / 输入类型</param>
        public Feature(string name, string defaultValue, ILocalizableString displayName = null, ILocalizableString description = null, FeatureScopes scope = FeatureScopes.All, IInputType inputType = null)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            Name = name;
            DisplayName = displayName;
            Description = description;
            Scope = scope;
            DefaultValue = defaultValue;
            InputType = inputType ?? new CheckboxInputType();

            _children = new List<Feature>();
            Attributes = new Dictionary<string, object>();
        }

        /// <summary>
        /// Adds a child feature.
        /// 添加一个子功能
        /// </summary>
        /// <returns>Returns newly created child feature / 返回新创建的子功能</returns>
        public Feature CreateChildFeature(string name, string defaultValue, ILocalizableString displayName = null, ILocalizableString description = null, FeatureScopes scope = FeatureScopes.All, IInputType inputType = null)
        {
            var feature = new Feature(name, defaultValue, displayName, description, scope, inputType) { Parent = this };
            _children.Add(feature);
            return feature;
        }

        public override string ToString()
        {
            return string.Format("[Feature: {0}]", Name);
        }
    }
}
