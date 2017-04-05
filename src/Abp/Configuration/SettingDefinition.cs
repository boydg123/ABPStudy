using System;
using Abp.Localization;

namespace Abp.Configuration
{
    /// <summary>
    /// Defines a setting.A setting is used to configure and change behavior of the application.
    /// 设置一个设置,一个设置（setting） 用于配置或改变应用的行为
    /// </summary>
    public class SettingDefinition
    {
        /// <summary>
        /// Unique name of the setting.
        /// 设置的唯一名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Display name of the setting.This can be used to show setting to the user.
        /// 显示名称,它可用于向用户显示
        /// </summary>
        public ILocalizableString DisplayName { get; set; }

        /// <summary>
        /// A brief description for this setting.
        /// 简要描述
        /// </summary>
        public ILocalizableString Description { get; set; }

        /// <summary>
        /// Scopes of this setting.Default value: <see cref="SettingScopes.Application"/>.
        /// 设置的作用域,默认值: <see cref="SettingScopes.Application"/>.
        /// </summary>
        public SettingScopes Scopes { get; set; }

        /// <summary>
        /// Is this setting inherited from parent scopes.Default: True.
        /// 是否继承父对象作用域,默认值: True.
        /// </summary>
        public bool IsInherited { get; set; }

        /// <summary>
        /// Gets/sets group for this setting.
        /// 获取或设置此setting的组
        /// </summary>
        public SettingDefinitionGroup Group { get; set; }

        /// <summary>
        /// Default value of the setting.
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Can clients see this setting and it's value.It maybe dangerous for some settings to be visible to clients (such as email server password).Default: false.
        /// 客户端是否可以看到该设置及其值,有些设置让客户端看到是很危险的（比如，邮箱，服务器密码）默认值为: false.
        /// </summary>
        public bool IsVisibleToClients { get; set; }

        /// <summary>
        /// Can be used to store a custom object related to this setting.
        /// 用于存储此设置相关连的自定义对象
        /// </summary>
        public object CustomData { get; set; }

        /// <summary>
        /// Creates a new <see cref="SettingDefinition"/> object.
        /// 构造函数
        /// </summary>
        /// <param name="name">Unique name of the setting / 名称</param>
        /// <param name="defaultValue">Default value of the setting / 默认值</param>
        /// <param name="displayName">Display name of the permission / 显示名称</param>
        /// <param name="group">Group of this setting / 组</param>
        /// <param name="description">A brief description for this setting / 简明描述</param>
        /// <param name="scopes">Scopes of this setting. Default value: <see cref="SettingScopes.Application"/>. / 设置作用域，默认为 <see cref="SettingScopes.Application"/>.</param>
        /// <param name="isVisibleToClients">Can clients see this setting and it's value. Default: false / 客户端是否可看到设置及其值. 默认为: false</param>
        /// <param name="isInherited">Is this setting inherited from parent scopes. Default: True. / 是否从父设置继承作用域，默认值为: True.</param>
        /// <param name="customData">Can be used to store a custom object related to this setting / 用于存储此设置相关连的自定义对象</param>
        public SettingDefinition(
            string name, 
            string defaultValue, 
            ILocalizableString displayName = null, 
            SettingDefinitionGroup group = null, 
            ILocalizableString description = null, 
            SettingScopes scopes = SettingScopes.Application, 
            bool isVisibleToClients = false, 
            bool isInherited = true,
            object customData = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            DefaultValue = defaultValue;
            DisplayName = displayName;
            Group = @group;
            Description = description;
            Scopes = scopes;
            IsVisibleToClients = isVisibleToClients;
            IsInherited = isInherited;
            CustomData = customData;
        }
    }
}
