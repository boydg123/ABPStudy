using System.Collections.Generic;

namespace Abp.Localization
{
    /// <summary>
    /// 语言管理器
    /// </summary>
    public interface ILanguageManager
    {
        /// <summary>
        /// 语言信息
        /// </summary>
        LanguageInfo CurrentLanguage { get; }

        /// <summary>
        /// 语言信息集合
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<LanguageInfo> GetLanguages();
    }
}