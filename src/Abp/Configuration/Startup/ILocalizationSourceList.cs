using System.Collections.Generic;
using Abp.Localization.Sources;

namespace Abp.Configuration.Startup
{
    /// <summary>
    /// Defines a specialized list to store <see cref="ILocalizationSource"/> object.
    /// 定义一个特定的列表存储 <see cref="ILocalizationSource"/> 对象.
    /// </summary>
    public interface ILocalizationSourceList : IList<ILocalizationSource>
    {
        /// <summary>
        /// Extensions for dictionay based localization sources.
        /// 基于 localization sources的字典扩展.
        /// </summary>
        IList<LocalizationSourceExtensionInfo> Extensions { get; }
    }
}