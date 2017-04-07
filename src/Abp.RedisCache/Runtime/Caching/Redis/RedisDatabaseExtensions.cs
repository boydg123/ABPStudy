using System;
using StackExchange.Redis;

namespace Abp.Runtime.Caching.Redis
{
    /// <summary>
    /// Extension methods for <see cref="IDatabase"/>.
    /// <see cref="IDatabase"/>的扩展方法
    /// </summary>
    internal static class RedisDatabaseExtensions
    {
        /// <summary>
        /// 使用前缀删除key
        /// </summary>
        /// <param name="database">数据库对象</param>
        /// <param name="prefix">前缀字符串</param>
        public static void KeyDeleteWithPrefix(this IDatabase database, string prefix)
        {
            if (database == null)
            {
                throw new ArgumentException("Database cannot be null", nameof(database));
            }

            if (string.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentException("Prefix cannot be empty", nameof(database));
            }

            database.ScriptEvaluate(@"
                local keys = redis.call('keys', ARGV[1]) 
                for i=1,#keys,5000 do 
                redis.call('del', unpack(keys, i, math.min(i+4999, #keys)))
                end", values: new RedisValue[] { prefix });
        }

        /// <summary>
        /// 根据前缀获取key数量
        /// </summary>
        /// <param name="database">数据库对象</param>
        /// <param name="prefix">前缀字符串</param>
        /// <returns>数量</returns>
        public static int KeyCount(this IDatabase database, string prefix)
        {
            if (database == null)
            {
                throw new ArgumentException("Database cannot be null", nameof(database));
            }

            if (string.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentException("Prefix cannot be empty", nameof(database));
            }

            var retVal = database.ScriptEvaluate("return table.getn(redis.call('keys', ARGV[1]))", values: new RedisValue[] { prefix });

            if (retVal.IsNull)
            {
                return 0;
            }

            return (int)retVal;
        }
    }
}
