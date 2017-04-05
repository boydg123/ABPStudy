using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Abp.Configuration.Startup;
using Abp.Dependency;

namespace Abp.Configuration
{
    /// <summary>
    /// Implements <see cref="ISettingDefinitionManager"/>.
    /// <see cref="ISettingDefinitionManager"/>接口实现
    /// </summary>
    internal class SettingDefinitionManager : ISettingDefinitionManager, ISingletonDependency
    {
        /// <summary>
        /// IOC管理器
        /// </summary>
        private readonly IIocManager _iocManager;

        /// <summary>
        /// 设置配置
        /// </summary>
        private readonly ISettingsConfiguration _settingsConfiguration;

        /// <summary>
        /// 设置定义集合
        /// </summary>
        private readonly IDictionary<string, SettingDefinition> _settings;

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        public SettingDefinitionManager(IIocManager iocManager, ISettingsConfiguration settingsConfiguration)
        {
            _iocManager = iocManager;
            _settingsConfiguration = settingsConfiguration;
            _settings = new Dictionary<string, SettingDefinition>();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            var context = new SettingDefinitionProviderContext(this);

            foreach (var providerType in _settingsConfiguration.Providers)
            {
                using (var provider = CreateProvider(providerType))
                {
                    foreach (var settings in provider.Object.GetSettingDefinitions(context))
                    {
                        _settings[settings.Name] = settings;
                    }
                }
            }
        }

        /// <summary>
        /// 获取给定名称的<see cref="SettingDefinition"/> 对象，如果不存在，抛出异常
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>设置定义对象</returns>
        public SettingDefinition GetSettingDefinition(string name)
        {
            SettingDefinition settingDefinition;
            if (!_settings.TryGetValue(name, out settingDefinition))
            {
                throw new AbpException("There is no setting defined with name: " + name);
            }

            return settingDefinition;
        }

        /// <summary>
        /// 获取所有的设置定义对象
        /// </summary>
        /// <returns>所有的设置定义</returns>
        public IReadOnlyList<SettingDefinition> GetAllSettingDefinitions()
        {
            return _settings.Values.ToImmutableList();
        }

        /// <summary>
        /// 创建提供者
        /// </summary>
        /// <param name="providerType"></param>
        /// <returns></returns>
        private IDisposableDependencyObjectWrapper<SettingProvider> CreateProvider(Type providerType)
        {
            _iocManager.RegisterIfNot(providerType, DependencyLifeStyle.Transient); //TODO: Needed?
            return _iocManager.ResolveAsDisposable<SettingProvider>(providerType);
        }
    }
}