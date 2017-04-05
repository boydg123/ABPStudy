using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Modules;

namespace Abp.PlugIns
{
    /// <summary>
    /// 插件源扩展
    /// </summary>
    public static class PlugInSourceExtensions
    {
        /// <summary>
        /// 根据所有的依赖获取模块
        /// </summary>
        /// <param name="plugInSource">插件源对象</param>
        /// <returns></returns>
        public static List<Type> GetModulesWithAllDependencies(this IPlugInSource plugInSource)
        {
            return plugInSource
                .GetModules()
                .SelectMany(AbpModule.FindDependedModuleTypesRecursivelyIncludingGivenModule)
                .Distinct()
                .ToList();
        }
    }
}