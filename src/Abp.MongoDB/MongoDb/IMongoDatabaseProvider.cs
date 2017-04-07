using MongoDB.Driver;

namespace Abp.MongoDb
{
    /// <summary>
    /// Defines interface to obtain a <see cref="MongoDatabase"/> object.
    /// 定义一个接口用于获得一个<see cref="MongoDatabase"/>对象
    /// </summary>
    public interface IMongoDatabaseProvider
    {
        /// <summary>
        /// Gets the <see cref="MongoDatabase"/>.
        /// 获取<see cref="MongoDatabase"/>对象
        /// </summary>
        MongoDatabase Database { get; }
    }
}