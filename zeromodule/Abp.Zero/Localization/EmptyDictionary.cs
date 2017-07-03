using System.Collections.Generic;
using System.Globalization;
using Abp.Localization.Dictionaries;

namespace Abp.Localization
{
    /// <summary>
    /// 本地化空字典
    /// </summary>
    internal class EmptyDictionary : ILocalizationDictionary
    {
        /// <summary>
        /// 区域信息
        /// </summary>
        public CultureInfo CultureInfo { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cultureInfo">区域信息</param>
        public EmptyDictionary(CultureInfo cultureInfo)
        {
            CultureInfo = cultureInfo;
        }
        /// <summary>
        /// 本地化字符串
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public LocalizedString GetOrNull(string name)
        {
            return null;
        }
        /// <summary>
        /// 获取所有本地化字符串
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<LocalizedString> GetAllStrings()
        {
            return new LocalizedString[0];
        }
        /// <summary>
        /// 名称索引
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public string this[string name]
        {
            get { return null; }
            set { }
        }
    }
}