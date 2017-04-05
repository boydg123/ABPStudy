using System;
using System.Collections.Generic;
using Abp.Collections.Extensions;

namespace Abp.Configuration
{
    /// <summary>
    /// Used to set/get custom configuration.
    /// 获取或设置自定义配置
    /// </summary>
    public class DictionaryBasedConfig : IDictionaryBasedConfig
    {
        /// <summary>
        /// Dictionary of custom configuration.
        /// 自定义配置字典
        /// </summary>
        protected Dictionary<string, object> CustomSettings { get; private set; }

        /// <summary>
        /// Gets/sets a config value.Returns null if no config with given name.
        /// 获取或设置一个配置值，如果给定名称的配置不存在，返回null
        /// </summary>
        /// <param name="name">Name of the config / 配置名称</param>
        /// <returns>Value of the config / 配置值</returns>
        public object this[string name]
        {
            get { return CustomSettings.GetOrDefault(name); }
            set { CustomSettings[name] = value; }
        }

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        protected DictionaryBasedConfig()
        {
            CustomSettings = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets a configuration value as a specific type.
        /// 获取一个指定类型的配置值
        /// </summary>
        /// <param name="name">Name of the config / 配置名称</param>
        /// <typeparam name="T">Type of the config / 配置类型</typeparam>
        /// <returns>Value of the configuration or null if not found / 配置值，如果不存在返回null</returns>
        public T Get<T>(string name)
        {
            var value = this[name];
            return value == null
                ? default(T)
                : (T) Convert.ChangeType(value, typeof (T));
        }

        /// <summary>
        /// Used to set a string named configuration.If there is already a configuration with same <paramref name="name"/>, it's overwritten.
        /// 设置一个字符串类型名称的配置，如果配置名称<paramref name="name"/>已经存在，将后被重写
        /// </summary>
        /// <param name="name">Unique name of the configuration / 唯一的配置名称</param>
        /// <param name="value">Value of the configuration / 配置值</param>
        public void Set<T>(string name, T value)
        {
            this[name] = value;
        }

        /// <summary>
        /// Gets a configuration object with given name.
        /// 获取给定名称的配置对象
        /// </summary>
        /// <param name="name">Unique name of the configuration / 唯一的配置名称</param>
        /// <returns>Value of the configuration or null if not found / 配置值,如查配置名称不存在返回null</returns>
        public object Get(string name)
        {
            return Get(name, null);
        }

        /// <summary>
        /// Gets a configuration object with given name.
        /// 获取给定名称的配置对象
        /// </summary>
        /// <param name="name">Unique name of the configuration / 唯一的配置名称</param>
        /// <param name="defaultValue">Default value of the object if can not found given configuration / 如果给定的配置名称不存在，返回的默认值</param>
        /// <returns>Value of the configuration or null if not found / 配置值,如查配置名称不存在返回默认值</returns>
        public object Get(string name, object defaultValue)
        {
            var value = this[name];
            if (value == null)
            {
                return defaultValue;
            }

            return this[name];
        }

        /// <summary>
        /// Gets a configuration object with given name.
        /// 获取给定名称的配置对象
        /// </summary>
        /// <typeparam name="T">Type of the object / 对象类型</typeparam>
        /// <param name="name">Unique name of the configuration / 唯一的配置名称</param>
        /// <param name="defaultValue">Default value of the object if can not found given configuration / 如果给定的配置名称不存在，返回的默认值</param>
        /// <returns>Value of the configuration or null if not found / 配置值,如查配置名称不存在返回默认值</returns>
        public T Get<T>(string name, T defaultValue)
        {
            return (T)Get(name, (object)defaultValue);
        }

        /// <summary>
        /// Gets a configuration object with given name.
        /// 获取给定名称的配置对象
        /// </summary>
        /// <typeparam name="T">Type of the object / 对象类型</typeparam>
        /// <param name="name">Unique name of the configuration / 唯一的配置名称</param>
        /// <param name="creator">The function that will be called to create if given configuration is not found / 委托，如果给定的配置名称不存在，将被调用</param>
        /// <returns>Value of the configuration or null if not found / 配置值,如查配置名称不存在返回委托创建的值</returns>
        public T GetOrCreate<T>(string name, Func<T> creator)
        {
            var value = Get(name);
            if (value == null)
            {
                value = creator();
                Set(name, value);
            }
            return (T) value;
        }
    }
}