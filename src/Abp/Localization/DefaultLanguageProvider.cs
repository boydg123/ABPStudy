using System.Collections.Generic;
using System.Collections.Immutable;
using Abp.Configuration.Startup;
using Abp.Dependency;

namespace Abp.Localization
{
    /// <summary>
    /// 默认语言提供者
    /// </summary>
    public class DefaultLanguageProvider : ILanguageProvider, ITransientDependency
    {
        /// <summary>
        /// 本地化配置
        /// </summary>
        private readonly ILocalizationConfiguration _configuration;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration">本地化配置</param>
        public DefaultLanguageProvider(ILocalizationConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 获取语言信息
        /// </summary>
        /// <returns>语言信息列表</returns>
        public IReadOnlyList<LanguageInfo> GetLanguages()
        {
            return _configuration.Languages.ToImmutableList();
        }
    }
}