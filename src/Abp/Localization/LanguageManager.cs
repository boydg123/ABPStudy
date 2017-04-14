using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Abp.Dependency;

namespace Abp.Localization
{
    /// <summary>
    /// 语言管理器
    /// </summary>
    public class LanguageManager : ILanguageManager, ITransientDependency
    {
        /// <summary>
        /// 区域信息
        /// </summary>
        public LanguageInfo CurrentLanguage { get { return GetCurrentLanguage(); } }

        /// <summary>
        /// 语言提供者
        /// </summary>
        private readonly ILanguageProvider _languageProvider;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="languageProvider">语言提供者</param>
        public LanguageManager(ILanguageProvider languageProvider)
        {
            _languageProvider = languageProvider;
        }

        /// <summary>
        /// 获取语言信息列表
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<LanguageInfo> GetLanguages()
        {
            return _languageProvider.GetLanguages();
        }

        /// <summary>
        /// 获取当前语言信息
        /// </summary>
        /// <returns></returns>
        private LanguageInfo GetCurrentLanguage()
        {
            var languages = _languageProvider.GetLanguages();
            if (languages.Count <= 0)
            {
                throw new AbpException("No language defined in this application.");
            }

            var currentCultureName = Thread.CurrentThread.CurrentUICulture.Name;

            //Try to find exact match
            var currentLanguage = languages.FirstOrDefault(l => l.Name == currentCultureName);
            if (currentLanguage != null)
            {
                return currentLanguage;
            }

            //Try to find best match
            currentLanguage = languages.FirstOrDefault(l => currentCultureName.StartsWith(l.Name));
            if (currentLanguage != null)
            {
                return currentLanguage;
            }

            //Try to find default language
            currentLanguage = languages.FirstOrDefault(l => l.IsDefault);
            if (currentLanguage != null)
            {
                return currentLanguage;
            }

            //Get first one
            return languages[0];
        }
    }
}