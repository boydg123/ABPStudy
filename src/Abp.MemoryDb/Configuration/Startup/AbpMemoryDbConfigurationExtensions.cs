using Abp.MemoryDb.Configuration;

namespace Abp.Configuration.Startup
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure ABP MemoryDb module.
    /// <see cref="IModuleConfigurations"/>的扩展方法用于配置ABP MemoryDB模块
    /// </summary>
    public static class AbpMemoryDbConfigurationExtensions
    {
        /// <summary>
        /// Used to configure ABP MemoryDb module.
        /// 用于配置ABP MemoryDB模块
        /// </summary>
        public static IAbpMemoryDbModuleConfiguration AbpMemoryDb(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<IAbpMemoryDbModuleConfiguration>();
        }
    }
}