using System.Reflection;

namespace Abp.Resources.Embedded
{
    /// <summary>
    /// Stores needed information of an embedded resource.
    /// 存储嵌入资源信息
    /// </summary>
    public class EmbeddedResourceInfo
    {
        /// <summary>
        /// Content of the resource file.
        /// 资源文件的内容
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// The assembly that contains the resource.
        /// 包含资源文件的程序集
        /// </summary>
        public Assembly Assembly { get; set; }

        internal EmbeddedResourceInfo(byte[] content, Assembly assembly)
        {
            Content = content;
            Assembly = assembly;
        }
    }
}