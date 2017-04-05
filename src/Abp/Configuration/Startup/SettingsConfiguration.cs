using Abp.Collections;

namespace Abp.Configuration.Startup
{
    /// <summary>
    /// 用于配置设置
    /// </summary>
    internal class SettingsConfiguration : ISettingsConfiguration
    {
        /// <summary>
        /// 设置提供者列表
        /// </summary>
        public ITypeList<SettingProvider> Providers { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SettingsConfiguration()
        {
            Providers = new TypeList<SettingProvider>();
        }
    }
}