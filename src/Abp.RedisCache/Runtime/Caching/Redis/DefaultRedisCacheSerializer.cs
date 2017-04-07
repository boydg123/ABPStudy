using System;
using Abp.Dependency;
using Abp.Json;
using StackExchange.Redis;

namespace Abp.Runtime.Caching.Redis
{
    /// <summary>
    /// Default implementation uses JSON as the underlying persistence mechanism.
    /// 使用JSON作为基础的持久化机制默认实现Redis缓存的序列化
    /// </summary>
    public class DefaultRedisCacheSerializer : IRedisCacheSerializer, ITransientDependency
    {
        /// <summary>
        /// Creates an instance of the object from its serialized string representation.
        /// 从序列化的字符串表现形式创建一个对象的示例
        /// </summary>
        /// <param name="objbyte">String representation of the object from the Redis server. / Redis服务器的对象字符串表现形式</param>
        /// <returns>Returns a newly constructed object. / 返回新建对象</returns>
        /// <seealso cref="IRedisCacheSerializer.Serialize" />
        public virtual object Deserialize(RedisValue objbyte)
        {
            return JsonSerializationHelper.DeserializeWithType(objbyte);
        }

        /// <summary>
        /// Produce a string representation of the supplied object.
        /// 产生所提供对象的字符串表现形式
        /// </summary>
        /// <param name="value">Instance to serialize. / 序列化的实例</param>
        /// <param name="type">Type of the object. / 对象的类型</param>
        /// <returns>Returns a string representing the object instance that can be placed into the Redis cache. / 返回一个字符串对象实例，可以放置到Redis缓存</returns>
        /// <seealso cref="IRedisCacheSerializer.Deserialize" />
        public virtual string Serialize(object value, Type type)
        {
            return JsonSerializationHelper.SerializeWithType(value, type);
        }
    }
}