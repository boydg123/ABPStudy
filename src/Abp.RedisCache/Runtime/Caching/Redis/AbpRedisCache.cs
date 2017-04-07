using System;
using Abp.Domain.Entities;
using StackExchange.Redis;

namespace Abp.Runtime.Caching.Redis
{
    /// <summary>
    /// Used to store cache in a Redis server.
    /// 用于在Redis服务中存储缓存
    /// </summary>
    public class AbpRedisCache : CacheBase
    {
        /// <summary>
        /// 数据库对象引用
        /// </summary>
        private readonly IDatabase _database;

        /// <summary>
        /// Redis缓存序列化器
        /// </summary>
        private readonly IRedisCacheSerializer _serializer;

        /// <summary>
        /// 构造函数.
        /// </summary>
        public AbpRedisCache(
            string name, 
            IAbpRedisCacheDatabaseProvider redisCacheDatabaseProvider, 
            IRedisCacheSerializer redisCacheSerializer)
            : base(name)
        {
            _database = redisCacheDatabaseProvider.GetDatabase();
            _serializer = redisCacheSerializer;
        }

        /// <summary>
        /// 从缓存获取数据，如果没有找到返回null
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        public override object GetOrDefault(string key)
        {
            var objbyte = _database.StringGet(GetLocalizedKey(key));
            return objbyte.HasValue ? Deserialize(objbyte) : null;
        }

        /// <summary>
        /// 通过键保存/覆盖缓存中的项
        /// /// 用一个过期时间最多(<paramref name="slidingExpireTime"/> 或 <paramref name="absoluteExpireTime"/>).
        /// 如果没有指定,则<see cref="DefaultAbsoluteExpireTime"/>将被使用，如果不为null。然而，<see cref="DefaultSlidingExpireTime"/>将被使用
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存的数据</param>
        /// <param name="slidingExpireTime">滑动过期时间</param>
        /// <param name="absoluteExpireTime">绝对过期时间</param>
        public override void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            if (value == null)
            {
                throw new AbpException("Can not insert null values to the cache!");
            }

            //TODO: This is a workaround for serialization problems of entities.
            //TODO: Normally, entities should not be stored in the cache, but currently Abp.Zero packages does it. It will be fixed in the future.
            var type = value.GetType();
            if (EntityHelper.IsEntity(type) && type.Assembly.FullName.Contains("EntityFrameworkDynamicProxies"))
            {
                type = type.BaseType;
            }

            _database.StringSet(
                GetLocalizedKey(key),
                Serialize(value, type),
                absoluteExpireTime ?? slidingExpireTime ?? DefaultAbsoluteExpireTime ?? DefaultSlidingExpireTime
                );
        }

        /// <summary>
        /// 移除指定键的缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        public override void Remove(string key)
        {
            _database.KeyDelete(GetLocalizedKey(key));
        }

        /// <summary>
        /// 清除所有的缓存数据
        /// </summary>
        public override void Clear()
        {
            _database.KeyDeleteWithPrefix(GetLocalizedKey("*"));
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        protected virtual string Serialize(object value, Type type)
        {
            return _serializer.Serialize(value, type);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="objbyte">反序列化的对象</param>
        /// <returns></returns>
        protected virtual object Deserialize(RedisValue objbyte)
        {
            return _serializer.Deserialize(objbyte);
        }

        /// <summary>
        /// 获取本地Key
        /// </summary>
        /// <param name="key">Key名称</param>
        /// <returns></returns>
        protected virtual string GetLocalizedKey(string key)
        {
            return "n:" + Name + ",c:" + key;
        }
    }
}
