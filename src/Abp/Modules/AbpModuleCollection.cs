using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Collections.Extensions;

namespace Abp.Modules
{
    /// <summary>
    /// Used to store AbpModuleInfo objects as a dictionary.
    /// 用一个字典存储AbpModuleInfo对象
    /// </summary>
    internal class AbpModuleCollection : List<AbpModuleInfo>
    {
        /// <summary>
        /// 启动模块类型
        /// </summary>
        public Type StartupModuleType { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="startupModuleType">启动模块类型</param>
        public AbpModuleCollection(Type startupModuleType)
        {
            StartupModuleType = startupModuleType;
        }

        /// <summary>
        /// Gets a reference to a module instance.
        /// 获取一个模块实例的引用
        /// </summary>
        /// <typeparam name="TModule">Module type / 模块类型</typeparam>
        /// <returns>Reference to the module instance / 模块实例的引用</returns>
        public TModule GetModule<TModule>() where TModule : AbpModule
        {
            var module = this.FirstOrDefault(m => m.Type == typeof(TModule));
            if (module == null)
            {
                throw new AbpException("Can not find module for " + typeof(TModule).FullName);
            }

            return (TModule)module.Instance;
        }

        /// <summary>
        /// Sorts modules according to dependencies.If module A depends on module B, A comes after B in the returned List.
        /// 按模块的依赖性对模块排序.如果A依赖于B，那么A将排在B之后
        /// </summary>
        /// <returns>Sorted list</returns>
        public List<AbpModuleInfo> GetSortedModuleListByDependency()
        {
            var sortedModules = this.SortByDependencies(x => x.Dependencies);
            EnsureKernelModuleToBeFirst(sortedModules);
            EnsureStartupModuleToBeLast(sortedModules, StartupModuleType);
            return sortedModules;
        }

        /// <summary>
        /// 确保内核模块优先
        /// </summary>
        /// <param name="modules">模块列表</param>
        public static void EnsureKernelModuleToBeFirst(List<AbpModuleInfo> modules)
        {
            var kernelModuleIndex = modules.FindIndex(m => m.Type == typeof(AbpKernelModule));
            if (kernelModuleIndex <= 0)
            {
                //It's already the first!
                return;
            }

            var kernelModule = modules[kernelModuleIndex];
            modules.RemoveAt(kernelModuleIndex);
            modules.Insert(0, kernelModule);
        }

        /// <summary>
        /// 确保启动模块最后
        /// </summary>
        /// <param name="modules">模块列表</param>
        /// <param name="startupModuleType">启动模块类型</param>
        public static void EnsureStartupModuleToBeLast(List<AbpModuleInfo> modules, Type startupModuleType)
        {
            var startupModuleIndex = modules.FindIndex(m => m.Type == startupModuleType);
            if (startupModuleIndex >= modules.Count - 1)
            {
                //It's already the last!
                return;
            }

            var startupModule = modules[startupModuleIndex];
            modules.RemoveAt(startupModuleIndex);
            modules.Add(startupModule);
        }

        /// <summary>
        /// 确保内核模块优先
        /// </summary>
        public void EnsureKernelModuleToBeFirst()
        {
            EnsureKernelModuleToBeFirst(this);
        }

        /// <summary>
        /// 确保启动模块最后
        /// </summary>
        public void EnsureStartupModuleToBeLast()
        {
            EnsureStartupModuleToBeLast(this, StartupModuleType);
        }
    }
}