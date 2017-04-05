using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;
using Abp.Dependency;
using Abp.IO.Extensions;

namespace Abp.Resources.Embedded
{
    /// <summary>
    /// 嵌入资源管理器
    /// </summary>
    public class EmbeddedResourceManager : IEmbeddedResourceManager, ISingletonDependency
    {
        private readonly ConcurrentDictionary<string, EmbeddedResourcePathInfo> _resourcePaths; //Key: Root path of the resource 资源的根路径
        private readonly ConcurrentDictionary<string, EmbeddedResourceInfo> _resourceCache; //Key: Root path of the resource 资源的根路径

        /// <summary>
        /// 构造函数.
        /// </summary>
        public EmbeddedResourceManager()
        {
            _resourcePaths = new ConcurrentDictionary<string, EmbeddedResourcePathInfo>();
            _resourceCache = new ConcurrentDictionary<string, EmbeddedResourceInfo>();
        }

        /// <summary>
        /// 尽可能地暴露一个文件夹内的所有文件（包含子文件夹）
        /// </summary>
        /// <param name="rootPath">
        /// 客户端看到的根文件夹路径，
        /// 它是任意深度的值
        /// </param>
        /// <param name="assembly">包含资源文件的程序集</param>
        /// <param name="resourceNamespace">匹配根路径的，<paramref name="assembly"/>中的命名空间</param>
        public void ExposeResources(string rootPath, Assembly assembly, string resourceNamespace)
        {
            if (_resourcePaths.ContainsKey(rootPath))
            {
                throw new AbpException("There is already an embedded resource with given rootPath: " + rootPath);
            }

            _resourcePaths[rootPath] = new EmbeddedResourcePathInfo(rootPath, assembly, resourceNamespace);
        }

        /// <summary>
        /// 获取一个嵌入的资源文件
        /// </summary>
        /// <param name="fullPath">资源文件的完整路径</param>
        /// <returns>资源文件</returns>
        public EmbeddedResourceInfo GetResource(string fullPath)
        {
            //Get from cache if exists!
            //从缓存中获取（如果存在）
            if (_resourceCache.ContainsKey(fullPath))
            {
                return _resourceCache[fullPath];
            }

            var pathInfo = GetPathInfoForFullPath(fullPath);

            using (var stream = pathInfo.Assembly.GetManifestResourceStream(pathInfo.FindManifestName(fullPath)))
            {
                if (stream == null)
                {
                    throw new AbpException("There is no exposed embedded resource for " + fullPath);
                }

                return _resourceCache[fullPath] = new EmbeddedResourceInfo(stream.GetAllBytes(), pathInfo.Assembly);
            }
        }

        /// <summary>
        /// 为完整路径获取路径信息
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        private EmbeddedResourcePathInfo GetPathInfoForFullPath(string fullPath)
        {
            foreach (var resourcePathInfo in _resourcePaths.Values.ToImmutableList()) //TODO@hikalkan: Test for multi-threading (possible multiple enumeration problem).
            {
                if (fullPath.StartsWith(resourcePathInfo.Path))
                {
                    return resourcePathInfo;
                }
            }

            throw new AbpException("There is no exposed embedded resource for: " + fullPath);
        }
    }
}