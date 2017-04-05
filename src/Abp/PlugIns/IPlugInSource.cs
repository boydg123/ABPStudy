using System;
using System.Collections.Generic;

namespace Abp.PlugIns
{
    /// <summary>
    /// 插件源
    /// </summary>
    public interface IPlugInSource
    {
        /// <summary>
        /// 获取模块列表
        /// </summary>
        /// <returns>模块类型列表</returns>
        List<Type> GetModules();
    }
}