using System;

namespace Abp.Configuration
{
    /// <summary>
    /// Defines interface to use a dictionary to make configurations.
    /// 定义一个接口，使用字典来实现配置
    /// </summary>
    public interface IDictionaryBasedConfig
    {
        /// <summary>
        /// Used to set a string named configuration.If there is already a configuration with same <paramref name="name"/>, it's overwritten.
        /// 设置一个配置名称，如果名称<paramref name="name"/>已经存在，则重写它
        /// </summary>
        /// <param name="name">Unique name of the configuration / 配置名称</param>
        /// <param name="value">Value of the configuration / 配置值</param>
        /// <returns>Returns the passed <paramref name="value"/>  / 返回传递值</returns>
        void Set<T>(string name, T value);

        /// <summary>
        /// Gets a configuration object with given name.
        /// 获取给定名称的配置对象
        /// </summary>
        /// <param name="name">Unique name of the configuration / 配置名称</param>
        /// <returns>Value of the configuration or null if not found / 配置值，如果不存在，返回null</returns>
        object Get(string name);

        /// <summary>
        /// Gets a configuration object with given name.
        /// 获取给定名称的配置对象
        /// </summary>
        /// <typeparam name="T">Type of the object / 配置对象类型</typeparam>
        /// <param name="name">Unique name of the configuration / 配置名称</param>
        /// <returns>Value of the configuration or null if not found / 配置值，如果不存在，返回null</returns>
        T Get<T>(string name);

        /// <summary>
        /// Gets a configuration object with given name.
        /// 获取给定名称的配置对象
        /// </summary>
        /// <param name="name">Unique name of the configuration / 配置名称</param>
        /// <param name="defaultValue">Default value of the object if can not found given configuration / 如果不存在，返回的默认值</param>
        /// <returns>Value of the configuration or null if not found / 配置值，如果不存在，返回给定的默认值</returns>
        object Get(string name, object defaultValue);

        /// <summary>
        /// Gets a configuration object with given name.
        /// 获取给定名称的配置对象
        /// </summary>
        /// <typeparam name="T">Type of the object / 配置对象类型</typeparam>
        /// <param name="name">Unique name of the configuration / 配置名称</param>
        /// <param name="defaultValue">Default value of the object if can not found given configuration / 如果不存在，返回的默认值</param>
        /// <returns>Value of the configuration or null if not found / 配置值，如果不存在，返回给定的默认值</returns>
        T Get<T>(string name, T defaultValue);

        /// <summary>
        /// Gets a configuration object with given name.
        /// 获取给定名称的配置对象
        /// </summary>
        /// <typeparam name="T">Type of the object / 配置对象类型</typeparam>
        /// <param name="name">Unique name of the configuration / 配置名称</param>
        /// <param name="creator">The function that will be called to create if given configuration is not found / 如果不存在，调用该委托</param>
        /// <returns>Value of the configuration or null if not found / 配置值，如果不存在，返回委托创建的值</returns>
        T GetOrCreate<T>(string name, Func<T> creator);
    }
}