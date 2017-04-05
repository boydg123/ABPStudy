using System;
using System.Collections.Generic;
using System.IO;
using Abp.Collections.Extensions;
using Abp.Modules;
using Abp.Reflection;

namespace Abp.PlugIns
{
    /// <summary>
    /// 插件文件夹
    /// </summary>
    public class FolderPlugInSource : IPlugInSource
    {
        /// <summary>
        /// 文件夹
        /// </summary>
        public string Folder { get; }

        /// <summary>
        /// 搜索目录枚举
        /// </summary>
        public SearchOption SearchOption { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="folder">文件夹</param>
        /// <param name="searchOption">搜索目录(当前目录搜索)</param>
        public FolderPlugInSource(string folder, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            Folder = folder;
            SearchOption = searchOption;
        }

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <returns>模块类型列表</returns>
        public List<Type> GetModules()
        {
            var modules = new List<Type>();

            var assemblies = AssemblyHelper.GetAllAssembliesInFolder(Folder, SearchOption);
            foreach (var assembly in assemblies)
            {
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (AbpModule.IsAbpModule(type))
                        {
                            modules.AddIfNotContains(type);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new AbpInitializationException("Could not get module types from assembly: " + assembly.FullName, ex);
                }
            }

            return modules;
        }
    }
}