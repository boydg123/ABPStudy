using Abp.MongoDb.Configuration;

namespace Abp.Configuration.Startup
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure ABP MongoDb module.
    /// <see cref="IModuleConfigurations"/>的扩展方法用于允许配置ABP MongoDB模块
    /// </summary>
    public static class AbpMongoDbConfigurationExtensions
    {
        /// <summary>
        /// Used to configure ABP MongoDb module.
        /// 用于配置ABP MongoDB模块
        /// </summary>
        public static IAbpMongoDbModuleConfiguration AbpMongoDb(this IModuleConfigurations configurations)
        {
            return configurations.AbpConfiguration.Get<IAbpMongoDbModuleConfiguration>();
        }
    }
}