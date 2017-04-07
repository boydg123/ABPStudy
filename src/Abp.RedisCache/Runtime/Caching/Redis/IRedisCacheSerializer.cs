using System;
using StackExchange.Redis;

namespace Abp.Runtime.Caching.Redis
{
    /// <summary>
    /// Interface to be implemented by all custom (de)serialization methods used when persisting and retrieving objects from the Redis cache.
    /// 所有自定义序列化(反序列化)方法要实现的接口。当从Redis缓存中保留和检索对象的时候使用。    
    /// </summary>
    public interface IRedisCacheSerializer
    {
        /// <summary>
        /// Creates an instance of the object from its serialized string representation.
        /// 从序列化的字符串表现形式创建一个对象的示例
        /// </summary>
        /// <param name="objbyte">String representation of the object from the Redis server. / Redis服务器的对象字符串表现形式</param>
        /// <returns>Returns a newly constructed object. / 返回新建对象</returns>
        /// <seealso cref="Serialize" />
        object Deserialize(RedisValue objbyte);

        /// <summary>
        /// Produce a string representation of the supplied object.
        /// 产生所提供对象的字符串表现形式
        /// </summary>
        /// <param name="value">Instance to serialize. / 序列化的实例</param>
        /// <param name="type">Type of the object. / 对象的类型</param>
        /// <returns>Returns a string representing the object instance that can be placed into the Redis cache. / 返回一个字符串对象实例，可以放置到Redis缓存</returns>
        /// <seealso cref="Deserialize" />
        string Serialize(object value, Type type);
    }
}