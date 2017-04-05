using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Abp.Reflection
{
    /// <summary>
    /// 程序集帮助累
    /// </summary>
    internal static class AssemblyHelper
    {
        /// <summary>
        /// 获取指定目录
        /// </summary>
        /// <param name="folderPath">指定目录</param>
        /// <param name="searchOption">指定是搜索当前目录，还是搜索当前目录及其所有子目录</param>
        /// <returns>程序集列表</returns>
        public static List<Assembly> GetAllAssembliesInFolder(string folderPath, SearchOption searchOption)
        {
            var assemblyFiles = Directory
                .EnumerateFiles(folderPath, "*.*", searchOption)
                .Where(s => s.EndsWith(".dll") || s.EndsWith(".exe"));

            return assemblyFiles.Select(Assembly.LoadFile).ToList();
        }
    }
}
