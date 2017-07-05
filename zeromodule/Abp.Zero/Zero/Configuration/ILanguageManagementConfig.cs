using Abp.Localization.Dictionaries;

namespace Abp.Zero.Configuration
{
    /// <summary>
    /// 语言管理配置
    /// </summary>
    public interface ILanguageManagementConfig
    {
        /// <summary>
        /// Enables the database localization.Replaces all <see cref="IDictionaryBasedLocalizationSource"/> localization sources with database based localization source.
        /// 启用数据库本地化。通过数据库基础本地化源替换所有的<see cref="IDictionaryBasedLocalizationSource"/>本地化源
        /// </summary>
        void EnableDbLocalization();
    }
}