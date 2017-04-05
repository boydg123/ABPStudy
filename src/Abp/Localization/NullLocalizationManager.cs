using System.Collections.Generic;
using System.Threading;
using Abp.Localization.Sources;

namespace Abp.Localization
{
    /// <summary>
    /// 本地化管理器的null对象实现
    /// </summary>
    public class NullLocalizationManager : ILocalizationManager
    {
        /// <summary>
        /// Singleton instance.
        /// 单例
        /// </summary>
        public static NullLocalizationManager Instance { get { return SingletonInstance; } }
        private static readonly NullLocalizationManager SingletonInstance = new NullLocalizationManager();

        /// <summary>
        /// 当前语言
        /// </summary>
        public LanguageInfo CurrentLanguage { get { return new LanguageInfo(Thread.CurrentThread.CurrentUICulture.Name, Thread.CurrentThread.CurrentUICulture.DisplayName); } }

        /// <summary>
        /// 空语言数组
        /// </summary>
        private readonly IReadOnlyList<LanguageInfo> _emptyLanguageArray = new LanguageInfo[0];

        /// <summary>
        /// 空本地化源数组
        /// </summary>
        private readonly IReadOnlyList<ILocalizationSource> _emptyLocalizationSourceArray = new ILocalizationSource[0];

        /// <summary>
        /// 构造函数
        /// </summary>
        private NullLocalizationManager()
        {
            
        }

        /// <summary>
        /// 获取所有的语言
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<LanguageInfo> GetAllLanguages()
        {
            return _emptyLanguageArray;
        }

        /// <summary>
        /// 获取本地化源
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ILocalizationSource GetSource(string name)
        {
            return NullLocalizationSource.Instance;
        }

        /// <summary>
        /// 获取所有的本地化源
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<ILocalizationSource> GetAllSources()
        {
            return _emptyLocalizationSourceArray;
        }
    }
}