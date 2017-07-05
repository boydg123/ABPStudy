using Abp.Configuration.Startup;

namespace Abp.Zero.Configuration
{
    /// <summary>
    /// Extension methods for module zero configurations.
    /// module zero配置的扩展方法
    /// </summary>
    public static class ModuleZeroConfigurationExtensions
    {
        /// <summary>
        /// 用于配置module zero
        /// </summary>
        /// <param name="moduleConfigurations"></param>
        /// <returns></returns>
        public static IAbpZeroConfig Zero(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.Get<IAbpZeroConfig>();
        }
    }
}