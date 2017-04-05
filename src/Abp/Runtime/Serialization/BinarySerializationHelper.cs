using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Abp.Runtime.Serialization
{
    /// <summary>
    /// This class is used to simplify serialization/deserialization operations.Uses .NET binary serialization.
    /// 这个类是用来简化序列化/反序列化操作。使用.NET二进制序列化。
    /// </summary>
    public static class BinarySerializationHelper
    {
        /// <summary>
        /// Serializes an object and returns as a byte array.
        /// 序列化一个对象并且返回一个字节数组
        /// </summary>
        /// <param name="obj">object to be serialized / 将被序列化的对象</param>
        /// <returns>bytes of object / 字节对象</returns>
        public static byte[] Serialize(object obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                CreateBinaryFormatter().Serialize(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Serializes an object into a stream.
        /// 将对象序列化成流
        /// </summary>
        /// <param name="obj">object to be serialized / 将被序列化的对象</param>
        /// <param name="stream">Stream to serialize in / 将要序列化的流</param>
        /// <returns>bytes of object / 字节对象</returns>
        public static void Serialize(object obj, Stream stream)
        {
            CreateBinaryFormatter().Serialize(stream, obj);
        }

        /// <summary>
        /// Deserializes an object from given byte array.
        /// 从给定的字节数组中反序列化对象
        /// </summary>
        /// <param name="bytes">The byte array that contains object / 包含对象的字节数组</param>
        /// <returns>deserialized object / 反序列化对象</returns>
        public static object Deserialize(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
            {
                return CreateBinaryFormatter().Deserialize(memoryStream);
            }
        }

        /// <summary>
        /// Deserializes an object from given stream.
        /// 从给定的流中反序列化一个对象
        /// </summary>
        /// <param name="stream">The stream that contains object / 包含对象的流</param>
        /// <returns>deserialized object / 反序列化对象</returns> 
        public static object Deserialize(Stream stream)
        {
            return CreateBinaryFormatter().Deserialize(stream);
        }

        /// <summary>
        /// Deserializes an object from given byte array.
        /// 从给定的字节数组中反序列化对象
        /// Difference from <see cref="Deserialize(byte[])"/> is that; this method can also deserialize
        /// types that are defined in dynamically loaded assemblies (like PlugIns).
        /// 不同于<see cref="Deserialize(byte[])"/>，这个方法也可以反序列化那些定义在动态加载的程序集中的类型(插件)
        /// </summary>
        /// <param name="bytes">The byte array that contains object / 包含对象的字节数组</param>
        /// <returns>deserialized object / 反序列化对象</returns>        
        public static object DeserializeExtended(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
            {
                return CreateBinaryFormatter(true).Deserialize(memoryStream);
            }
        }

        /// <summary>
        /// Deserializes an object from given stream.
        /// 从给定的流中反序列化对象
        /// Difference from <see cref="Deserialize(Stream)"/> is that; this method can also deserialize
        /// types that are defined in dynamically loaded assemblies (like PlugIns).
        /// 不同于<see cref="Deserialize(Stream)"/>，这个方法也可以反序列化那些定义在动态加载的程序集中的类型(插件)
        /// </summary>
        /// <param name="stream">The stream that contains object / 包含对象的流</param>
        /// <returns>deserialized object / 反序列化对象</returns> 
        public static object DeserializeExtended(Stream stream)
        {
            return CreateBinaryFormatter(true).Deserialize(stream);
        }

        private static BinaryFormatter CreateBinaryFormatter(bool extended = false)
        {
            if (extended)
            {
                return new BinaryFormatter
                {
                    AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple,
                    Binder = new ExtendedSerializationBinder()
                };
            }
            else
            {
                return new BinaryFormatter();
            }
        }

        /// <summary>
        /// This class is used in deserializing to allow deserializing objects that are defined in assemlies that are load in runtime (like PlugIns).
        /// 这个类用于反序列化那些允许在运行时中反序列化对象定义的加载的程序集(如插件)
        /// </summary>
        private sealed class ExtendedSerializationBinder : SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                var toAssemblyName = assemblyName.Split(',')[0];
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (assembly.FullName.Split(',')[0] == toAssemblyName)
                    {
                        return assembly.GetType(typeName);
                    }
                }

                return Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));
            }
        }
    }
}
