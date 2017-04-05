using System.Reflection;

namespace Abp.Resources.Embedded
{
    /// <summary>
    /// Provides infrastructure to use any type of resources (files) embedded into assemblies.
    /// 提供使用程序集中任意类型资源文件的基础架构
    /// </summary>
    public interface IEmbeddedResourceManager
    {
        /// <summary>
        /// Makes possible to expose all files in a folder (and subfolders recursively).
        /// 尽可能地暴露一个文件夹内的所有文件（及其子文件夹递归）
        /// </summary>
        /// <param name="rootPath">
        /// Root folder path to be seen by clients. / 客户端看到的根文件夹路径
        /// This is an arbitrary value with any deep. / 它是任意深度的值
        /// </param>
        /// <param name="assembly">The assembly contains resources. / 包含资源文件的程序集</param>
        /// <param name="resourceNamespace">Namespace in the <paramref name="assembly"/> that matches to the root path / 匹配根路径的，<paramref name="assembly"/>中的命名空间</param>
        void ExposeResources(string rootPath, Assembly assembly, string resourceNamespace);

        /// <summary>
        /// Used to get an embedded resource file.
        /// 获取一个嵌入的资源文件
        /// </summary>
        /// <param name="fullResourcePath">Full path of the resource / 资源文件的完整路径</param>
        /// <returns>The resource / 资源文件</returns>
        EmbeddedResourceInfo GetResource(string fullResourcePath);
    }
}