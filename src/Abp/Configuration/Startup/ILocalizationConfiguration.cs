using System.Collections.Generic;
using Abp.Localization;

namespace Abp.Configuration.Startup
{
    /// <summary>
    /// Used for localization configurations.
    /// 用于本地化配置
    /// </summary>
    public interface ILocalizationConfiguration
    {
        /// <summary>
        /// Used to set languages available for this application.
        /// 用于设置应用可用的语言
        /// </summary>
        IList<LanguageInfo> Languages { get; }

        /// <summary>
        /// List of localization sources.
        /// 本地资源列表
        /// </summary>
        ILocalizationSourceList Sources { get; }

        /// <summary>
        /// Used to enable/disable localization system.
        /// 用于启用/禁用本地化系统
        /// Default: true.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// If this is set to true, the given text (name) is returned.if not found in the localization source. That prevent exceptions if given name is not defined in the localization sources.
        /// 如果设置为true，则指定文本(name)被返回，如果在本地源没有找到，如果给定的名称没有在本地资源中定义则防止异常
        /// Also writes a warning log.Default: true.
        /// 也会写一个警告日志。默认：true
        /// </summary>
        bool ReturnGivenTextIfNotFound { get; set; }

        /// <summary>
        /// It returns the given text by wrapping with [ and ] chars.if not found in the localization source.
        /// 如果在本地源中没有找到，则它返回给定的文本使用[and]字符包装
        /// This is considered only if <see cref="ReturnGivenTextIfNotFound"/> is true.Default: true.
        /// 这被认为是仅当<see cref="ReturnGivenTextIfNotFound"/>为true。默认：true
        /// </summary>
        bool WrapGivenTextIfNotFound { get; set; }

        /// <summary>
        /// It returns the given text by converting string from 'PascalCase' to a 'Sentense case'.if not found in the localization source.
        /// 它返回给定的文本通过转换字符串从'PascalCase'到'Sentense case'，如果在本地源中没有找到
        /// This is considered only if <see cref="ReturnGivenTextIfNotFound"/> is true.Default: true.
        /// 这被认为是仅当<see cref="ReturnGivenTextIfNotFound"/>为true。默认：true
        /// </summary>
        bool HumanizeTextIfNotFound { get; set; }
    }
}
