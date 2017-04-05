using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Abp.Modules;

namespace Abp.Reflection
{
    /// <summary>
    /// ABP框架程序集查找器
    /// </summary>
    public class AbpAssemblyFinder : IAssemblyFinder
    {
        /// <summary>
        /// ABP框架模块管理器
        /// </summary>
        private readonly IAbpModuleManager _moduleManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="moduleManager">ABP框架模块管理器</param>
        public AbpAssemblyFinder(IAbpModuleManager moduleManager)
        {
            _moduleManager = moduleManager;
        }

        /// <summary>
        /// 获取所哟程序集
        /// </summary>
        /// <returns></returns>
        public List<Assembly> GetAllAssemblies()
        {
            var assemblies = new List<Assembly>();

            foreach (var module in _moduleManager.Modules)
            {
                assemblies.Add(module.Assembly);
                assemblies.AddRange(module.Instance.GetAdditionalAssemblies());
            }

            return assemblies.Distinct().ToList();
        }
    }
}