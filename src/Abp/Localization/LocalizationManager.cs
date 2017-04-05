using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Localization.Dictionaries;
using Abp.Localization.Sources;
using Castle.Core.Logging;

namespace Abp.Localization
{
    /// <summary>
    /// 本地化管理器
    /// </summary>
    internal class LocalizationManager : ILocalizationManager
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// 语言管理器
        /// </summary>
        private readonly ILanguageManager _languageManager;

        /// <summary>
        /// 本地化配置
        /// </summary>
        private readonly ILocalizationConfiguration _configuration;

        /// <summary>
        /// IOC解析器
        /// </summary>
        private readonly IIocResolver _iocResolver;

        /// <summary>
        /// 本地化源
        /// </summary>
        private readonly IDictionary<string, ILocalizationSource> _sources;

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        public LocalizationManager(
            ILanguageManager languageManager,
            ILocalizationConfiguration configuration, 
            IIocResolver iocResolver)
        {
            Logger = NullLogger.Instance;
            _languageManager = languageManager;
            _configuration = configuration;
            _iocResolver = iocResolver;
            _sources = new Dictionary<string, ILocalizationSource>();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            InitializeSources();
        }

        /// <summary>
        /// 初始化源
        /// </summary>
        private void InitializeSources()
        {
            if (!_configuration.IsEnabled)
            {
                Logger.Debug("Localization disabled.");
                return;
            }

            Logger.Debug(string.Format("Initializing {0} localization sources.", _configuration.Sources.Count));
            foreach (var source in _configuration.Sources)
            {
                if (_sources.ContainsKey(source.Name))
                {
                    throw new AbpException("There are more than one source with name: " + source.Name + "! Source name must be unique!");
                }

                _sources[source.Name] = source;
                source.Initialize(_configuration, _iocResolver);

                //Extending dictionaries
                if (source is IDictionaryBasedLocalizationSource)
                {
                    var dictionaryBasedSource = source as IDictionaryBasedLocalizationSource;
                    var extensions = _configuration.Sources.Extensions.Where(e => e.SourceName == source.Name).ToList();
                    foreach (var extension in extensions)
                    {
                        extension.DictionaryProvider.Initialize(source.Name);
                        foreach (var extensionDictionary in extension.DictionaryProvider.Dictionaries.Values)
                        {
                            dictionaryBasedSource.Extend(extensionDictionary);
                        }
                    }
                }

                Logger.Debug("Initialized localization source: " + source.Name);
            }
        }

        /// <summary>
        /// Gets a localization source with name.
        /// 获取给定名称的本地化源
        /// </summary>
        /// <param name="name">Unique name of the localization source / 源名称</param>
        /// <returns>The localization source / 本地化源</returns>
        public ILocalizationSource GetSource(string name)
        {
            if (!_configuration.IsEnabled)
            {
                return NullLocalizationSource.Instance;
            }

            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            ILocalizationSource source;
            if (!_sources.TryGetValue(name, out source))
            {
                throw new AbpException("Can not find a source with name: " + name);
            }

            return source;
        }

        /// <summary>
        /// Gets all registered localization sources.
        /// 获取所有注册了的localization sources.
        /// </summary>
        /// <returns>List of sources / sources列表</returns>
        public IReadOnlyList<ILocalizationSource> GetAllSources()
        {
            return _sources.Values.ToImmutableList();
        }
    }
}