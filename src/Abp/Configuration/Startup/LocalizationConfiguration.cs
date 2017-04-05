using System.Collections.Generic;
using Abp.Localization;

namespace Abp.Configuration.Startup
{
    /// <summary>
    /// Used for localization configurations.
    /// 用于本地化配置
    /// </summary>
    internal class LocalizationConfiguration : ILocalizationConfiguration
    {
        /// <summary>
        /// 为应用设置有效的语言
        /// </summary>
        public IList<LanguageInfo> Languages { get; private set; }

        /// <summary>
        /// 本地化源列表
        /// </summary>
        public ILocalizationSourceList Sources { get; private set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <inheritdoc/>
        public bool ReturnGivenTextIfNotFound { get; set; }

        /// <inheritdoc/>
        public bool WrapGivenTextIfNotFound { get; set; }

        /// <inheritdoc/>
        public bool HumanizeTextIfNotFound { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LocalizationConfiguration()
        {
            Languages = new List<LanguageInfo>();
            Sources = new LocalizationSourceList();

            IsEnabled = true;
            ReturnGivenTextIfNotFound = true;
            WrapGivenTextIfNotFound = true;
            HumanizeTextIfNotFound = true;
        }
    }
}
