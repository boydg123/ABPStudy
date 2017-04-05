namespace Abp.Configuration.Startup
{
    /// <summary>
    /// Used to provide a way to configure modules.
    /// 用于提供一种方式来配置模块
    /// Create entension methods to this class to be used over <see cref="IAbpStartupConfiguration.Modules"/> object.
    /// 创建该类的扩展方法，可用于<see cref="IAbpStartupConfiguration.Modules"/> 对象 
    /// </summary>
    public interface IModuleConfigurations
    {
        /// <summary>
        /// Gets the ABP configuration object.
        /// 获取abp配置对象
        /// </summary>
        IAbpStartupConfiguration AbpConfiguration { get; }
    }
}