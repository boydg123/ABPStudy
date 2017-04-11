using System;
using Newtonsoft.Json;

namespace Abp.Json
{
    /// <summary>
    /// Defines helper methods to work with JSON.
    /// Json辅助类
    /// </summary>
    public static class JsonSerializationHelper
    {
        /// <summary>
        /// 类型分割符
        /// </summary>
        private const char TypeSeperator = '|';

        /// <summary>
        /// Serializes an object with a type information included.
        /// 序列化一个包含类型信息的对象
        /// So, it can be deserialized using <see cref="DeserializeWithType"/> method later.
        /// 所以，之后它能使用<see cref="DeserializeWithType"/>方法反序列化
        /// </summary>
        public static string SerializeWithType(object obj)
        {
            return SerializeWithType(obj, obj.GetType());
        }

        /// <summary>
        /// Serializes an object with a type information included.
        /// 序列化一个包含类型信息的对象
        /// So, it can be deserialized using <see cref="DeserializeWithType"/> method later.
        /// 所以，之后它能使用<see cref="DeserializeWithType"/>方法反序列化
        /// </summary>
        public static string SerializeWithType(object obj, Type type)
        {
            var serialized = obj.ToJsonString();

            return string.Format(
                "{0}{1}{2}",
                type.AssemblyQualifiedName,
                TypeSeperator,
                serialized
                );
        }

        /// <summary>
        /// Deserializes an object serialized with <see cref="SerializeWithType(object)"/> methods.
        /// 使用<see cref="SerializeWithType(object)"/>方法反序列化一个对象
        /// </summary>
        public static T DeserializeWithType<T>(string serializedObj)
        {
            return (T)DeserializeWithType(serializedObj);
        }

        /// <summary>
        /// Deserializes an object serialized with <see cref="SerializeWithType(object)"/> methods.
        /// 使用<see cref="SerializeWithType(object)"/>方法反序列化一个对象
        /// </summary>
        public static object DeserializeWithType(string serializedObj)
        {
            var typeSeperatorIndex = serializedObj.IndexOf(TypeSeperator);
            var type = Type.GetType(serializedObj.Substring(0, typeSeperatorIndex));
            var serialized = serializedObj.Substring(typeSeperatorIndex + 1);

            var options = new JsonSerializerSettings();
            options.Converters.Insert(0, new AbpDateTimeConverter());

            return JsonConvert.DeserializeObject(serialized, type, options);
        }
    }
}