using System.Collections.Generic;
using System.Linq;
using Abp.Runtime.Session;
using Abp.Threading;

namespace Abp.Localization
{
    /// <summary>
    /// Implements <see cref="ILanguageProvider"/> to get languages from <see cref="IApplicationLanguageManager"/>.
    /// <see cref="ILanguageProvider"/>的实现，从<see cref="IApplicationLanguageManager"/>获取语言
    /// </summary>
    public class ApplicationLanguageProvider : ILanguageProvider
    {
        /// <summary>
        /// ABP Session的引用
        /// </summary>
        public IAbpSession AbpSession { get; set; }
        /// <summary>
        /// 应用程序语言管理
        /// </summary>
        private readonly IApplicationLanguageManager _applicationLanguageManager;

        /// <summary>
        /// 构造函数.
        /// </summary>
        public ApplicationLanguageProvider(IApplicationLanguageManager applicationLanguageManager)
        {
            _applicationLanguageManager = applicationLanguageManager;

            AbpSession = NullAbpSession.Instance;
        }

        /// <summary>
        /// 为当前商户获取默认语言.
        /// </summary>
        public IReadOnlyList<LanguageInfo> GetLanguages()
        {
            var languageInfos = AsyncHelper.RunSync(() => _applicationLanguageManager.GetLanguagesAsync(AbpSession.TenantId))
                    .OrderBy(l => l.DisplayName)
                    .Select(l => l.ToLanguageInfo())
                    .ToList();

            SetDefaultLanguage(languageInfos);

            return languageInfos;
        }
        /// <summary>
        /// 为当前商户设置默认语言
        /// </summary>
        /// <param name="languageInfos"></param>
        private void SetDefaultLanguage(List<LanguageInfo> languageInfos)
        {
            if (languageInfos.Count <= 0)
            {
                return;
            }

            var defaultLanguage = AsyncHelper.RunSync(() => _applicationLanguageManager.GetDefaultLanguageOrNullAsync(AbpSession.TenantId));
            if (defaultLanguage == null)
            {
                languageInfos[0].IsDefault = true;
                return;
            }
            
            var languageInfo = languageInfos.FirstOrDefault(l => l.Name == defaultLanguage.Name);
            if (languageInfo == null)
            {
                languageInfos[0].IsDefault = true;
                return;
            }

            languageInfo.IsDefault = true;
        }
    }
}