namespace Abp.MongoDb.Configuration
{
    /// <summary>
    /// ABP MongoDB模块配置实现
    /// </summary>
    internal class AbpMongoDbModuleConfiguration : IAbpMongoDbModuleConfiguration
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatatabaseName { get; set; }
    }
}