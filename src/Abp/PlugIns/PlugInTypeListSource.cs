using System;
using System.Collections.Generic;
using System.Linq;

namespace Abp.PlugIns
{
    /// <summary>
    /// 插件类型列表
    /// </summary>
    public class PlugInTypeListSource : IPlugInSource
    {
        /// <summary>
        /// 类型列表
        /// </summary>
        private readonly Type[] _moduleTypes;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="moduleTypes">类型列表</param>
        public PlugInTypeListSource(params Type[] moduleTypes)
        {
            _moduleTypes = moduleTypes;
        }

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <returns>模块类型列表</returns>
        public List<Type> GetModules()
        {
            return _moduleTypes.ToList();
        }
    }
}