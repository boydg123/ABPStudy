using System.Linq;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Localization;
using Abp.Localization.Dictionaries;
using Castle.Core.Logging;

namespace Abp.Zero.Configuration
{
    /// <summary>
    /// 语言管理配置
    /// </summary>
    internal class LanguageManagementConfig : ILanguageManagementConfig
    {
        /// <summary>
        /// 日志引用
        /// </summary>
        public ILogger Logger { get; set; }
        /// <summary>
        /// IOC引用
        /// </summary>
        private readonly IIocManager _iocManager;
        /// <summary>
        /// ABP启动配置
        /// </summary>
        private readonly IAbpStartupConfiguration _configuration;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iocManager">IOC引用</param>
        /// <param name="configuration">ABP启动配置</param>
        public LanguageManagementConfig(IIocManager iocManager, IAbpStartupConfiguration configuration)
        {
            _iocManager = iocManager;
            _configuration = configuration;

            Logger = NullLogger.Instance;
        }
        /// <summary>
        /// 启用数据库本地化
        /// </summary>
        public void EnableDbLocalization()
        {
            _iocManager.Register<ILanguageProvider, ApplicationLanguageProvider>(DependencyLifeStyle.Transient);

            var sources = _configuration
                .Localization
                .Sources
                .Where(s => s is IDictionaryBasedLocalizationSource)
                .Cast<IDictionaryBasedLocalizationSource>()
                .ToList();
            
            foreach (var source in sources)
            {
                _configuration.Localization.Sources.Remove(source);
                _configuration.Localization.Sources.Add(
                    new MultiTenantLocalizationSource(
                        source.Name,
                        new MultiTenantLocalizationDictionaryProvider(
                            source.DictionaryProvider,
                            _iocManager
                            )
                        )
                    );

                Logger.DebugFormat("Converted {0} ({1}) to MultiTenantLocalizationSource", source.Name, source.GetType());
            }
        }
    }
}