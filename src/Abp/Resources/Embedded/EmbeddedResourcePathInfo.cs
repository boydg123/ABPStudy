using System.Linq;
using System.Reflection;
using System.Text;

namespace Abp.Resources.Embedded
{
    /// <summary>
    /// 嵌入资源路径信息
    /// </summary>
    internal class EmbeddedResourcePathInfo
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// 程序集
        /// </summary>
        public Assembly Assembly { get; private set; }

        /// <summary>
        /// 资源命名空间
        /// </summary>
        public string ResourceNamespace { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="assembly"></param>
        /// <param name="resourceNamespace"></param>
        public EmbeddedResourcePathInfo(string path, Assembly assembly, string resourceNamespace)
        {
            Path = path;
            Assembly = assembly;
            ResourceNamespace = resourceNamespace;
        }

        /// <summary>
        /// 查找清单名称
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public string FindManifestName(string fullPath)
        {
            var relativeResourcePath = fullPath.Substring(Path.Length + 1);
            relativeResourcePath = NormalizeResourcePath(relativeResourcePath);
            return string.Format("{0}.{1}", ResourceNamespace, relativeResourcePath.Replace("/", "."));
        }

        /// <summary>
        /// 规范化资源路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string NormalizeResourcePath(string path)
        {
            var pathFolders = path.Split('/');
            if (pathFolders.Length < 2)
            {
                return path;
            }
            
            var sb = new StringBuilder();

            for (var i = 0; i < pathFolders.Length - 1; i++)
            {
                sb.Append(NormalizeFolderName(pathFolders[i]) + "/");
            }

            sb.Append(pathFolders.Last()); //Append file name

            return sb.ToString();
        }

        /// <summary>
        /// 规范化文件夹名称
        /// </summary>
        /// <param name="pathPart"></param>
        /// <returns></returns>
        private static string NormalizeFolderName(string pathPart)
        {
            //TODO: Implement all rules of .NET
            return pathPart.Replace('-', '_');
        }
    }
}