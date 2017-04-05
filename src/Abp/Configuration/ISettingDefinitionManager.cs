using System.Collections.Generic;

namespace Abp.Configuration
{
    /// <summary>
    /// Defines setting definition manager.
    /// 设置定义管理器
    /// </summary>
    public interface ISettingDefinitionManager
    {
        /// <summary>
        /// Gets the <see cref="SettingDefinition"/> object with given unique name.Throws exception if can not find the setting.
        /// 获取给定名称的<see cref="SettingDefinition"/> 对象,如果不存在，抛出异常
        /// </summary>
        /// <param name="name">Unique name of the setting / 名称</param>
        /// <returns>The <see cref="SettingDefinition"/> object. / <see cref="SettingDefinition"/>设置定义对象</returns>
        SettingDefinition GetSettingDefinition(string name);

        /// <summary>
        /// Gets a list of all setting definitions.
        /// 获取所有的设置定义对象
        /// </summary>
        /// <returns>All settings. / 所有的设置定义</returns>
        IReadOnlyList<SettingDefinition> GetAllSettingDefinitions();
    }
}
