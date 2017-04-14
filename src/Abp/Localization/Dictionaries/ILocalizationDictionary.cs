using System.Collections.Generic;
using System.Globalization;

namespace Abp.Localization.Dictionaries
{
    /// <summary>
    /// Represents a dictionary that is used to find a localized string.
    /// 表示用于查找本地化字符串的字典
    /// </summary>
    public interface ILocalizationDictionary
    {
        /// <summary>
        /// Culture of the dictionary.
        /// 字典的区域
        /// </summary>
        CultureInfo CultureInfo { get; }

        /// <summary>
        /// Gets/sets a string for this dictionary with given name (key).
        /// 获取或设置给字键名称的字符串
        /// </summary>
        /// <param name="name">Name to get/set / 用于获取或设置的名称</param>
        string this[string name] { get; set; }

        /// <summary>
        /// Gets a <see cref="LocalizedString"/> for given <paramref name="name"/>.
        /// 获取一个给定名称 <paramref name="name"/>的<see cref="LocalizedString"/> .
        /// </summary>
        /// <param name="name">Name (key) to get localized string / 名称</param>
        /// <returns>The localized string or null if not found in this dictionary / 本地化字符串，如果在字典中不存在指定的名称，返回null</returns>
        LocalizedString GetOrNull(string name);

        /// <summary>
        /// Gets a list of all strings in this dictionary.
        /// 获取字典中的本地化字符串
        /// </summary>
        /// <returns>List of all <see cref="LocalizedString"/> object / 本地化 <see cref="LocalizedString"/> 列表对象</returns>
        IReadOnlyList<LocalizedString> GetAllStrings();
    }
}