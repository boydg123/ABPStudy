using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Abp.Collections.Extensions;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Castle.Core.Logging;

namespace Abp.Modules
{
    /// <summary>
    /// This class must be implemented by all module definition classes.
    /// 所有的模块定义类必须继承些类
    /// </summary>
    /// <remarks>
    /// A module definition class is generally located in it's own assembly and implements some action in module events on application startup and shutdown.It also defines depended modules.
    /// 模块定义类一般位于自己的程序集中，在应用程序启动和关闭时在模块事件中实现一些操作，并定义依赖模块。
    /// </remarks>
    public abstract class AbpModule
    {
        /// <summary>
        /// Gets a reference to the IOC manager.
        /// 获取IOC管理器的一个引用
        /// </summary>
        protected internal IIocManager IocManager { get; internal set; }

        /// <summary>
        /// Gets a reference to the ABP configuration.
        /// 获取ABP启动配置信息的一个引用
        /// </summary>
        protected internal IAbpStartupConfiguration Configuration { get; internal set; }

        /// <summary>
        /// Gets or sets the logger.
        /// 获取或设置日志记录者
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected AbpModule()
        {
            Logger = NullLogger.Instance;
        }

        /// <summary>
        /// This is the first event called on application startup. Codes can be placed here to run before dependency injection registrations.
        /// 这是应用启动调用的第一个事件，这里面的代码，会在依赖注入注册之前运行
        /// </summary>
        public virtual void PreInitialize()
        {

        }

        /// <summary>
        /// This method is used to register dependencies for this module.
        /// 这个方法用于模块的依赖注册
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// This method is called lastly on application startup.
        /// 这个方法在应用启动最后被调用
        /// </summary>
        public virtual void PostInitialize()
        {
            
        }

        /// <summary>
        /// This method is called when the application is being shutdown.
        /// 这方法，在应用正在关闭时调用
        /// </summary>
        public virtual void Shutdown()
        {
            
        }

        /// <summary>
        /// 获取额外的程序集集合
        /// </summary>
        /// <returns></returns>
        public virtual Assembly[] GetAdditionalAssemblies()
        {
            return new Assembly[0];
        }

        /// <summary>
        /// Checks if given type is an Abp module class.
        /// 检查给定的类型是ABP组建类
        /// </summary>
        /// <param name="type">Type to check / 待检查的类型</param>
        public static bool IsAbpModule(Type type)
        {
            return
                type.IsClass &&
                !type.IsAbstract &&
                !type.IsGenericType &&
                typeof(AbpModule).IsAssignableFrom(type);
        }

        /// <summary>
        /// Finds direct depended modules of a module (excluding given module).
        /// 查找一个模块的依赖模块(不包含给定模块)
        /// </summary>
        public static List<Type> FindDependedModuleTypes(Type moduleType)
        {
            if (!IsAbpModule(moduleType))
            {
                throw new AbpInitializationException("This type is not an ABP module: " + moduleType.AssemblyQualifiedName);
            }

            var list = new List<Type>();

            if (moduleType.IsDefined(typeof(DependsOnAttribute), true))
            {
                var dependsOnAttributes = moduleType.GetCustomAttributes(typeof(DependsOnAttribute), true).Cast<DependsOnAttribute>();
                foreach (var dependsOnAttribute in dependsOnAttributes)
                {
                    foreach (var dependedModuleType in dependsOnAttribute.DependedModuleTypes)
                    {
                        list.Add(dependedModuleType);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 递归查询给定模块的依赖模块
        /// </summary>
        /// <param name="moduleType">模块类型</param>
        /// <returns></returns>
        public static List<Type> FindDependedModuleTypesRecursivelyIncludingGivenModule(Type moduleType)
        {
            var list = new List<Type>();
            AddModuleAndDependenciesResursively(list, moduleType);
            list.AddIfNotContains(typeof(AbpKernelModule));
            return list;
        }

        /// <summary>
        /// 递归添加至模块依赖列表
        /// </summary>
        /// <param name="modules">模块依赖列表</param>
        /// <param name="module">模块类型</param>
        private static void AddModuleAndDependenciesResursively(List<Type> modules, Type module)
        {
            if (!IsAbpModule(module))
            {
                throw new AbpInitializationException("This type is not an ABP module: " + module.AssemblyQualifiedName);
            }

            if (modules.Contains(module))
            {
                return;
            }

            modules.Add(module);

            var dependedModules = FindDependedModuleTypes(module);
            foreach (var dependedModule in dependedModules)
            {
                AddModuleAndDependenciesResursively(modules, dependedModule);
            }
        }
    }
}
