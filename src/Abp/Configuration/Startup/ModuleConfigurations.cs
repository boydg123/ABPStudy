namespace Abp.Configuration.Startup
{
    /// <summary>
    /// 用于提供一种方式来配置模块,创建该类的扩展方法，可用于<see cref="IAbpStartupConfiguration.Modules"/> 对象 
    /// </summary>
    internal class ModuleConfigurations : IModuleConfigurations
    {
        /// <summary>
        /// ABP启动配置信息引用
        /// </summary>
        public IAbpStartupConfiguration AbpConfiguration { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="abpConfiguration"></param>
        public ModuleConfigurations(IAbpStartupConfiguration abpConfiguration)
        {
            AbpConfiguration = abpConfiguration;
        }
    }
}