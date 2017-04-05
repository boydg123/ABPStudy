using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;

namespace Abp.Localization.Dictionaries
{
    /// <summary>
    /// Represents a simple implementation of <see cref="ILocalizationDictionary"/> interface.
    /// <see cref="ILocalizationDictionary"/>接口简单的实现
    /// </summary>
    public class LocalizationDictionary : ILocalizationDictionary, IEnumerable<LocalizedString>
    {
        /// <summary>
        /// 字典的文化
        /// </summary>
        public CultureInfo CultureInfo { get; private set; }

        /// <summary>
        /// 获取或设置给字键名称的字符串
        /// </summary>
        /// <param name="name">用于获取或设置的名称</param>
        /// <returns></returns>
        public virtual string this[string name]
        {
            get
            {
                var localizedString = GetOrNull(name);
                return localizedString == null ? null : localizedString.Value;
            }
            set
            {
                _dictionary[name] = new LocalizedString(name, value, CultureInfo);
            }
        }

        private readonly Dictionary<string, LocalizedString> _dictionary;

        /// <summary>
        /// Creates a new <see cref="LocalizationDictionary"/> object.
        /// 构造函数
        /// </summary>
        /// <param name="cultureInfo">Culture of the dictionary / 字典的文化</param>
        public LocalizationDictionary(CultureInfo cultureInfo)
        {
            CultureInfo = cultureInfo;
            _dictionary = new Dictionary<string, LocalizedString>();
        }

        /// <summary>
        /// 获取一个给定名称 <paramref name="name"/>的<see cref="LocalizedString"/> .
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>本地化字符串，如果在字典中不存在指定的名称，返回null</returns>
        public virtual LocalizedString GetOrNull(string name)
        {
            LocalizedString localizedString;
            return _dictionary.TryGetValue(name, out localizedString) ? localizedString : null;
        }

        /// <summary>
        /// 获取字典中的本地化字符串
        /// </summary>
        /// <returns>本地化 <see cref="LocalizedString"/> 列表对象</returns>
        public virtual IReadOnlyList<LocalizedString> GetAllStrings()
        {
            return _dictionary.Values.ToImmutableList();
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举器
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator<LocalizedString> GetEnumerator()
        {
            return GetAllStrings().GetEnumerator();
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举器。
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetAllStrings().GetEnumerator();
        }

        /// <summary>
        /// 是否包含名称
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        protected bool Contains(string name)
        {
            return _dictionary.ContainsKey(name);
        }
    }
}