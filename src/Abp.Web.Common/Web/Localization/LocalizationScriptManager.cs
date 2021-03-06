﻿using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Abp.Dependency;
using Abp.Json;
using Abp.Localization;
using Abp.Runtime.Caching;

namespace Abp.Web.Localization
{
    /// <summary>
    /// 本地化脚本管理器实现(定义获取本地化Javascript脚本)
    /// </summary>
    internal class LocalizationScriptManager : ILocalizationScriptManager, ISingletonDependency
    {
        /// <summary>
        /// 本地化管理器
        /// </summary>
        private readonly ILocalizationManager _localizationManager;

        /// <summary>
        /// 缓存管理器
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// 语言管理器
        /// </summary>
        private readonly ILanguageManager _languageManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="localizationManager">本地化管理器</param>
        /// <param name="cacheManager">缓存管理器</param>
        /// <param name="languageManager">语言管理器</param>
        public LocalizationScriptManager(
            ILocalizationManager localizationManager, 
            ICacheManager cacheManager,
            ILanguageManager languageManager)
        {
            _localizationManager = localizationManager;
            _cacheManager = cacheManager;
            _languageManager = languageManager;
        }

        /// <summary>
        /// 获取包含当前区域中的所有本地化信息的JavaScript。
        /// </summary>
        /// <returns></returns>
        public string GetScript()
        {
            return GetScript(Thread.CurrentThread.CurrentUICulture);
        }

        /// <summary>
        /// 获取包含当前区域中的所有本地化信息的JavaScript。
        /// </summary>
        /// <param name="cultureInfo">区域信息</param>
        /// <returns></returns>
        public string GetScript(CultureInfo cultureInfo)
        {
            //NOTE: Disabled caching since it's not true (localization script is changed per user, per tenant, per culture...)
            return BuildAll(cultureInfo);
            //return _cacheManager.GetCache(AbpCacheNames.LocalizationScripts).Get(cultureInfo.Name, () => BuildAll(cultureInfo));
        }

        /// <summary>
        /// 构建JavaScript。
        /// </summary>
        /// <param name="cultureInfo">区域信息</param>
        /// <returns></returns>
        private string BuildAll(CultureInfo cultureInfo)
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");
            script.AppendLine();
            script.AppendLine("    abp.localization = abp.localization || {};");
            script.AppendLine();
            script.AppendLine("    abp.localization.currentCulture = {");
            script.AppendLine("        name: '" + cultureInfo.Name + "',");
            script.AppendLine("        displayName: '" + cultureInfo.DisplayName + "'");
            script.AppendLine("    };");
            script.AppendLine();
            script.Append("    abp.localization.languages = [");

            var languages = _languageManager.GetLanguages();
            for (var i = 0; i < languages.Count; i++)
            {
                var language = languages[i];

                script.AppendLine("{");
                script.AppendLine("        name: '" + language.Name + "',");
                script.AppendLine("        displayName: '" + language.DisplayName + "',");
                script.AppendLine("        icon: '" + language.Icon + "',");
                script.AppendLine("        isDefault: " + language.IsDefault.ToString().ToLower());
                script.Append("    }");

                if (i < languages.Count - 1)
                {
                    script.Append(" , ");
                }
            }

            script.AppendLine("];");
            script.AppendLine();

            if (languages.Count > 0)
            {
                var currentLanguage = _languageManager.CurrentLanguage;
                script.AppendLine("    abp.localization.currentLanguage = {");
                script.AppendLine("        name: '" + currentLanguage.Name + "',");
                script.AppendLine("        displayName: '" + currentLanguage.DisplayName + "',");
                script.AppendLine("        icon: '" + currentLanguage.Icon + "',");
                script.AppendLine("        isDefault: " + currentLanguage.IsDefault.ToString().ToLower());
                script.AppendLine("    };");
            }

            var sources = _localizationManager.GetAllSources().OrderBy(s => s.Name).ToArray();

            script.AppendLine();
            script.AppendLine("    abp.localization.sources = [");

            for (int i = 0; i < sources.Length; i++)
            {
                var source = sources[i];
                script.AppendLine("        {");
                script.AppendLine("            name: '" + source.Name + "',");
                script.AppendLine("            type: '" + source.GetType().Name + "'");
                script.AppendLine("        }" + (i < (sources.Length - 1) ? "," : ""));
            }

            script.AppendLine("    ];");

            script.AppendLine();
            script.AppendLine("    abp.localization.values = abp.localization.values || {};");
            script.AppendLine();

            foreach (var source in sources)
            {
                script.Append("    abp.localization.values['" + source.Name + "'] = ");

                var stringValues = source.GetAllStrings(cultureInfo).OrderBy(s => s.Name).ToList();
                var stringJson = stringValues
                    .ToDictionary(_ => _.Name, _ => _.Value)
                    .ToJsonString(indented: true);
                script.Append(stringJson);

                script.AppendLine(";");
                script.AppendLine();
            }

            script.AppendLine();
            script.Append("})();");

            return script.ToString();
        }
    }
}
