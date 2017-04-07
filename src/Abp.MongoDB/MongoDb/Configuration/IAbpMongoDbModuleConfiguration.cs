namespace Abp.MongoDb.Configuration
{
    /// <summary>
    /// ABP MongoDB模块配置接口
    /// </summary>
    public interface IAbpMongoDbModuleConfiguration
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        string DatatabaseName { get; set; }
    }
}
