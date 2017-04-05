using System.Collections.Generic;
using Abp.Localization.Sources;

namespace Abp.Localization
{
    /// <summary>
    /// This interface is used to manage localization system.
    /// 用于管理本地化系统的接口
    /// </summary>
    public interface ILocalizationManager
    {
        /// <summary>
        /// Gets a localization source with name.
        /// 通过名称获取一个localization source
        /// </summary>
        /// <param name="name">Unique name of the localization source / 本地化资源唯一的名称</param>
        /// <returns>The localization source / 返回获取到的localization source</returns>
        ILocalizationSource GetSource(string name);

        /// <summary>
        /// Gets all registered localization sources.
        /// 获取所有注册了的localization sources.
        /// </summary>
        /// <returns>List of sources / sources列表</returns>
        IReadOnlyList<ILocalizationSource> GetAllSources();
    }
}