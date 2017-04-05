using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Abp.Dependency;

namespace Abp.Application.Features
{
    /// <summary>
    /// Implements <see cref="IFeatureManager"/>.
    /// <see cref="IFeatureManager"/>的实现
    /// </summary>
    internal class FeatureManager : FeatureDefinitionContextBase, IFeatureManager, ISingletonDependency
    {

        /// <summary>
        /// IOC管理器
        /// </summary>
        private readonly IIocManager _iocManager;

        /// <summary>
        /// 功能配置
        /// </summary>
        private readonly IFeatureConfiguration _featureConfiguration;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iocManager">IOC管理器</param>
        /// <param name="featureConfiguration">功能配置</param>
        public FeatureManager(IIocManager iocManager, IFeatureConfiguration featureConfiguration)
        {
            _iocManager = iocManager;
            _featureConfiguration = featureConfiguration;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            foreach (var providerType in _featureConfiguration.Providers)
            {
                using (var provider = CreateProvider(providerType))
                {
                    provider.Object.SetFeatures(this);
                }
            }

            Features.AddAllFeatures();
        }

        /// <summary>
        /// 根据名称获取功能
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Feature Get(string name)
        {
            var feature = GetOrNull(name);
            if (feature == null)
            {
                throw new AbpException("There is no feature with name: " + name);
            }

            return feature;
        }

        /// <summary>
        /// 获取所有功能
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<Feature> GetAll()
        {
            return Features.Values.ToImmutableList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerType"></param>
        /// <returns></returns>
        private IDisposableDependencyObjectWrapper<FeatureProvider> CreateProvider(Type providerType)
        {
            _iocManager.RegisterIfNot(providerType); //TODO: Needed?
            return _iocManager.ResolveAsDisposable<FeatureProvider>(providerType);
        }
    }
}
