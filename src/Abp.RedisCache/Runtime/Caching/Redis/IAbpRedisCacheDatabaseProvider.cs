using StackExchange.Redis;

namespace Abp.Runtime.Caching.Redis
{
    /// <summary>
    /// Used to get <see cref="IDatabase"/> for Redis cache.
    /// 用于为Redis缓存获取<see cref="IDatabase"/>
    /// </summary>
    public interface IAbpRedisCacheDatabaseProvider 
    {
        /// <summary>
        /// Gets the database connection.
        /// 获取数据库连接
        /// </summary>
        IDatabase GetDatabase();
    }
}
