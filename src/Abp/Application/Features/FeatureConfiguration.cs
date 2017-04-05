using Abp.Collections;

namespace Abp.Application.Features
{
    /// <summary>
    /// 功能配置
    /// </summary>
    internal class FeatureConfiguration : IFeatureConfiguration
    {
        /// <summary>
        /// 功能提供者列表
        /// </summary>
        public ITypeList<FeatureProvider> Providers { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FeatureConfiguration()
        {
            Providers = new TypeList<FeatureProvider>();
        }
    }
}