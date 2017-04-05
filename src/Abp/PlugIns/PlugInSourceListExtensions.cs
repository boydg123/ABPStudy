using System;
using System.IO;

namespace Abp.PlugIns
{
    /// <summary>
    /// 插件源列表扩展
    /// </summary>
    public static class PlugInSourceListExtensions
    {
        /// <summary>
        /// 添加文件夹
        /// </summary>
        /// <param name="list">插件源列表</param>
        /// <param name="folder">文件夹名称</param>
        /// <param name="searchOption">搜索目录(当前目录搜索)</param>
        public static void AddFolder(this PlugInSourceList list, string folder, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            list.Add(new FolderPlugInSource(folder, searchOption));
        }

        /// <summary>
        /// 添加模块类型至插件列表
        /// </summary>
        /// <param name="list">插件源列表</param>
        /// <param name="moduleTypes">模块类型列表</param>
        public static void AddTypeList(this PlugInSourceList list, params Type[] moduleTypes)
        {
            list.Add(new PlugInTypeListSource(moduleTypes));
        }
    }
}