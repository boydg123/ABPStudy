using System;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Dependency.Installers;
using Abp.Modules;
using Abp.PlugIns;
using Castle.Core.Logging;
using Castle.MicroKernel.Registration;
using JetBrains.Annotations;

namespace Abp
{
    /// <summary>
    /// This is the main class that is responsible to start entire ABP system.
    /// Prepares dependency injection and registers core components needed for startup.
    /// It must be instantiated and initialized (see <see cref="Initialize"/>) first in an application.
    /// 这是一个主要的类，它负责启动整后abp系统。在启动中准备依赖注入和核心组件的注册 在一个应用中，
    /// 它必须最先实例化并初始化(查看 <see cref="Initialize"/>) 
    /// </summary>
    public class AbpBootstrapper : IDisposable
    {
        /// <summary>
        /// Get the startup module of the application which depends on other used modules.
        /// 获取依赖于其他使用模块的应用程序的启动模块
        /// </summary>
        public Type StartupModule { get; }

        /// <summary>
        /// A list of plug in folders.
        /// 插件文件夹列表
        /// </summary>
        public PlugInSourceList PlugInSources { get; }

        /// <summary>
        /// Gets IIocManager object used by this class.
        /// 获取这个类使用的IIocManager对象
        /// </summary>
        public IIocManager IocManager { get; }

        /// <summary>
        /// Is this object disposed before?
        /// 对象是否已经销毁
        /// </summary>
        protected bool IsDisposed;

        /// <summary>
        /// 模块管理器
        /// </summary>
        private AbpModuleManager _moduleManager;

        /// <summary>
        /// 日志对象引用
        /// </summary>
        private ILogger _logger;

        /// <summary>
        /// Creates a new <see cref="AbpBootstrapper"/> instance.
        /// 创建一个新的 <see cref="AbpBootstrapper"/> 实例.
        /// </summary>
        /// <param name="startupModule">Startup module of the application which depends on other used modules. Should be derived from <see cref="AbpModule"/>. / 启动模块的应用程序依赖于其他使用的模块。应该来自 <see cref="AbpModule"/></param>
        private AbpBootstrapper([NotNull] Type startupModule)
            : this(startupModule, Dependency.IocManager.Instance)
        {

        }

        /// <summary>
        /// Creates a new <see cref="AbpBootstrapper"/> instance.
        /// 创建一个新的 <see cref="AbpBootstrapper"/> 实例.
        /// </summary>
        /// <param name="startupModule">Startup module of the application which depends on other used modules. Should be derived from <see cref="AbpModule"/>. / 启动模块的应用程序依赖于其他使用的模块。应该来自 <see cref="AbpModule"/></param>
        /// <param name="iocManager">IIocManager that is used to bootstrap the ABP system / 用于引导ABP系统的IOC管理器</param>
        private AbpBootstrapper([NotNull] Type startupModule, [NotNull] IIocManager iocManager)
        {
            Check.NotNull(startupModule, nameof(startupModule));
            Check.NotNull(iocManager, nameof(iocManager));

            if (!typeof(AbpModule).IsAssignableFrom(startupModule))
            {
                throw new ArgumentException($"{nameof(startupModule)} should be derived from {nameof(AbpModule)}.");
            }

            StartupModule = startupModule;
            IocManager = iocManager;

            PlugInSources = new PlugInSourceList();
            _logger = NullLogger.Instance;
        }

        /// <summary>
        /// Creates a new <see cref="AbpBootstrapper"/> instance.
        /// 创建一个新的 <see cref="AbpBootstrapper"/> 实例.
        /// </summary>
        /// <typeparam name="TStartupModule">Startup module of the application which depends on other used modules. Should be derived from <see cref="AbpModule"/>. / 启动模块的应用程序依赖于其他使用的模块。应该来自 <see cref="AbpModule"/></typeparam>
        public static AbpBootstrapper Create<TStartupModule>()
            where TStartupModule : AbpModule
        {
            return new AbpBootstrapper(typeof(TStartupModule));
        }

        /// <summary>
        /// Creates a new <see cref="AbpBootstrapper"/> instance.
        /// 创建一个新的 <see cref="AbpBootstrapper"/> 实例.
        /// </summary>
        /// <typeparam name="TStartupModule">Startup module of the application which depends on other used modules. Should be derived from <see cref="AbpModule"/>. / 启动模块的应用程序依赖于其他使用的模块。应该来自 <see cref="AbpModule"/></typeparam>
        /// <param name="iocManager">IIocManager that is used to bootstrap the ABP system / 用于引导ABP系统的IOC管理器</param>
        public static AbpBootstrapper Create<TStartupModule>([NotNull] IIocManager iocManager)
            where TStartupModule : AbpModule
        {
            return new AbpBootstrapper(typeof(TStartupModule), iocManager);
        }

        /// <summary>
        /// Creates a new <see cref="AbpBootstrapper"/> instance.
        /// 创建一个新的 <see cref="AbpBootstrapper"/> 实例.
        /// </summary>
        /// <param name="startupModule">Startup module of the application which depends on other used modules. Should be derived from <see cref="AbpModule"/>. / 启动模块的应用程序依赖于其他使用的模块。应该来自 <see cref="AbpModule"/></param>
        public static AbpBootstrapper Create([NotNull] Type startupModule)
        {
            return new AbpBootstrapper(startupModule);
        }

        /// <summary>
        /// Creates a new <see cref="AbpBootstrapper"/> instance.
        /// 创建一个新的 <see cref="AbpBootstrapper"/> 实例.
        /// </summary>
        /// <param name="startupModule">Startup module of the application which depends on other used modules. Should be derived from <see cref="AbpModule"/>. / 启动模块的应用程序依赖于其他使用的模块。应该来自 <see cref="AbpModule"/></param>
        /// <param name="iocManager">IIocManager that is used to bootstrap the ABP system / 用于引导ABP系统的IOC管理器</param>
        public static AbpBootstrapper Create([NotNull] Type startupModule, [NotNull] IIocManager iocManager)
        {
            return new AbpBootstrapper(startupModule, iocManager);
        }

        /// <summary>
        /// Initializes the ABP system.
        /// 初始化ABP系统
        /// </summary>
        public virtual void Initialize()
        {
            ResolveLogger();

            try
            {
                RegisterBootstrapper();
                IocManager.IocContainer.Install(new AbpCoreInstaller());

                IocManager.Resolve<AbpPlugInManager>().PlugInSources.AddRange(PlugInSources);
                IocManager.Resolve<AbpStartupConfiguration>().Initialize();

                _moduleManager = IocManager.Resolve<AbpModuleManager>();
                _moduleManager.Initialize(StartupModule);
                _moduleManager.StartModules();
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex.ToString(), ex);
                throw;
            }
        }

        /// <summary>
        /// 解析日志
        /// </summary>
        private void ResolveLogger()
        {
            if (IocManager.IsRegistered<ILoggerFactory>())
            {
                _logger = IocManager.Resolve<ILoggerFactory>().Create(typeof(AbpBootstrapper));
            }
        }

        /// <summary>
        /// 注册引导程序
        /// </summary>
        private void RegisterBootstrapper()
        {
            if (!IocManager.IsRegistered<AbpBootstrapper>())
            {
                IocManager.IocContainer.Register(
                    Component.For<AbpBootstrapper>().Instance(this)
                    );
            }
        }

        /// <summary>
        /// Disposes the ABP system.
        /// 销毁 ABP 系统.
        /// </summary>
        public virtual void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            _moduleManager?.ShutdownModules();
        }
    }
}
