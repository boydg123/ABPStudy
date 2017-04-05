using System.Collections.Generic;

namespace Abp.Localization
{
    /// <summary>
    /// 语言提供者
    /// </summary>
    public interface ILanguageProvider
    {
        /// <summary>
        /// 语言信息集合
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<LanguageInfo> GetLanguages();
    }
}