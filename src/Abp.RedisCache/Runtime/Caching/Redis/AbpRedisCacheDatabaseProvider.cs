using System;
using Abp.Dependency;
using StackExchange.Redis;

namespace Abp.Runtime.Caching.Redis
{
    /// <summary>
    /// Implements <see cref="IAbpRedisCacheDatabaseProvider"/>.
    /// <see cref="IAbpRedisCacheDatabaseProvider"/>的实现
    /// </summary>
    public class AbpRedisCacheDatabaseProvider : IAbpRedisCacheDatabaseProvider, ISingletonDependency
    {
        /// <summary>
        /// ABP Redis缓存选项
        /// </summary>
        private readonly AbpRedisCacheOptions _options;

        /// <summary>
        /// 连接复用器
        /// </summary>
        private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbpRedisCacheDatabaseProvider"/> class.
        /// 构造函数
        /// </summary>
        public AbpRedisCacheDatabaseProvider(AbpRedisCacheOptions options)
        {
            _options = options;
            _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer);
        }

        /// <summary>
        /// Gets the database connection.
        /// 获取数据库连接
        /// </summary>
        public IDatabase GetDatabase()
        {
            return _connectionMultiplexer.Value.GetDatabase(_options.DatabaseId);
        }

        /// <summary>
        /// 创建连接复用器
        /// </summary>
        /// <returns></returns>
        private ConnectionMultiplexer CreateConnectionMultiplexer()
        {
            return ConnectionMultiplexer.Connect(_options.ConnectionString);
        }
    }
}
