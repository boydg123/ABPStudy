using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Abp.Collections.Extensions;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.PlugIns;
using Castle.Core.Logging;

namespace Abp.Modules
{
    /// <summary>
    /// This class is used to manage modules.
    /// ABP模块管理类
    /// </summary>
    public class AbpModuleManager : IAbpModuleManager
    {
        /// <summary>
        /// ABP启动模块
        /// </summary>
        public AbpModuleInfo StartupModule { get; private set; }

        /// <summary>
        /// 模块信息只读列表
        /// </summary>
        public IReadOnlyList<AbpModuleInfo> Modules => _modules.ToImmutableList();

        /// <summary>
        /// 日志写入对象
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// ABP模块集合
        /// </summary>
        private AbpModuleCollection _modules;

        /// <summary>
        /// IOC管理器
        /// </summary>
        private readonly IIocManager _iocManager;
        /// <summary>
        /// ABP插件管理器
        /// </summary>
        private readonly IAbpPlugInManager _abpPlugInManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iocManager">IOC管理器</param>
        /// <param name="abpPlugInManager">ABP插件管理器</param>
        public AbpModuleManager(IIocManager iocManager, IAbpPlugInManager abpPlugInManager)
        {
            _iocManager = iocManager;
            _abpPlugInManager = abpPlugInManager;
            Logger = NullLogger.Instance;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="startupModule">初始化模块类型</param>
        public virtual void Initialize(Type startupModule)
        {
            _modules = new AbpModuleCollection(startupModule);
            LoadAllModules();
        }

        /// <summary>
        /// 启动相关模块
        /// </summary>
        public virtual void StartModules()
        {
            var sortedModules = _modules.GetSortedModuleListByDependency();
            sortedModules.ForEach(module => module.Instance.PreInitialize());
            sortedModules.ForEach(module => module.Instance.Initialize());
            sortedModules.ForEach(module => module.Instance.PostInitialize());
        }

        /// <summary>
        /// 关闭相关模块
        /// </summary>
        public virtual void ShutdownModules()
        {
            Logger.Debug("Shutting down has been started");

            var sortedModules = _modules.GetSortedModuleListByDependency();
            sortedModules.Reverse();
            sortedModules.ForEach(sm => sm.Instance.Shutdown());

            Logger.Debug("Shutting down completed.");
        }

        /// <summary>
        /// 加载所有模块
        /// </summary>
        private void LoadAllModules()
        {
            Logger.Debug("Loading Abp modules...");

            var moduleTypes = FindAllModules().Distinct().ToList();

            Logger.Debug("Found " + moduleTypes.Count + " ABP modules in total.");

            RegisterModules(moduleTypes);
            CreateModules(moduleTypes);

            _modules.EnsureKernelModuleToBeFirst();
            _modules.EnsureStartupModuleToBeLast();

            SetDependencies();

            Logger.DebugFormat("{0} modules loaded.", _modules.Count);
        }

        /// <summary>
        /// 查询所有模块
        /// </summary>
        /// <returns></returns>
        private List<Type> FindAllModules()
        {
            var modules = AbpModule.FindDependedModuleTypesRecursivelyIncludingGivenModule(_modules.StartupModuleType);

            _abpPlugInManager
                .PlugInSources
                .GetAllModules()
                .ForEach(m => modules.AddIfNotContains(m));

            return modules;
        }

        /// <summary>
        /// 创建模块
        /// </summary>
        /// <param name="moduleTypes">模块集合</param>
        private void CreateModules(ICollection<Type> moduleTypes)
        {
            foreach (var moduleType in moduleTypes)
            {
                var moduleObject = _iocManager.Resolve(moduleType) as AbpModule;
                if (moduleObject == null)
                {
                    throw new AbpInitializationException("This type is not an ABP module: " + moduleType.AssemblyQualifiedName);
                }

                moduleObject.IocManager = _iocManager;
                moduleObject.Configuration = _iocManager.Resolve<IAbpStartupConfiguration>();

                var moduleInfo = new AbpModuleInfo(moduleType, moduleObject);

                _modules.Add(moduleInfo);

                if (moduleType == _modules.StartupModuleType)
                {
                    StartupModule = moduleInfo;
                }

                Logger.DebugFormat("Loaded module: " + moduleType.AssemblyQualifiedName);
            }
        }

        /// <summary>
        /// 注册模块
        /// </summary>
        /// <param name="moduleTypes">模块集合</param>
        private void RegisterModules(ICollection<Type> moduleTypes)
        {
            foreach (var moduleType in moduleTypes)
            {
                _iocManager.RegisterIfNot(moduleType);
            }
        }

        /// <summary>
        /// 设置依赖性
        /// </summary>
        private void SetDependencies()
        {
            foreach (var moduleInfo in _modules)
            {
                moduleInfo.Dependencies.Clear();

                //Set dependencies for defined DependsOnAttribute attribute(s).
                //从定义的DependsOnAttribute特性来设置模块的依赖
                foreach (var dependedModuleType in AbpModule.FindDependedModuleTypes(moduleInfo.Type))
                {
                    var dependedModuleInfo = _modules.FirstOrDefault(m => m.Type == dependedModuleType);
                    if (dependedModuleInfo == null)
                    {
                        throw new AbpInitializationException("Could not find a depended module " + dependedModuleType.AssemblyQualifiedName + " for " + moduleInfo.Type.AssemblyQualifiedName);
                    }

                    if ((moduleInfo.Dependencies.FirstOrDefault(dm => dm.Type == dependedModuleType) == null))
                    {
                        moduleInfo.Dependencies.Add(dependedModuleInfo);
                    }
                }
            }
        }
    }
}
