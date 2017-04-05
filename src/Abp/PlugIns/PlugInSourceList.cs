using System;
using System.Collections.Generic;
using System.Linq;

namespace Abp.PlugIns
{
    /// <summary>
    /// 插件源列表
    /// </summary>
    public class PlugInSourceList : List<IPlugInSource>
    {
        /// <summary>
        /// 获取所有模块
        /// </summary>
        /// <returns>模块类型列表</returns>
        public List<Type> GetAllModules()
        {
            return this
                .SelectMany(pluginSource => pluginSource.GetModulesWithAllDependencies())
                .Distinct()
                .ToList();
        }
    }
}