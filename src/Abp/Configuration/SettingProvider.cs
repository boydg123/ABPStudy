using System.Collections.Generic;
using Abp.Dependency;

namespace Abp.Configuration
{
    /// <summary>
    /// Inherit this class to define settings for a module/application.
    /// 继承这个类，为模块/应用定义设置
    /// </summary>
    public abstract class SettingProvider : ITransientDependency
    {
        /// <summary>
        /// Gets all setting definitions provided by this provider.
        /// 获取该提供者提供的所有设置定义
        /// </summary>
        /// <returns>List of settings / 设置列表</returns>
        public abstract IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context);
    }
}