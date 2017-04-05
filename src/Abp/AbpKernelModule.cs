using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using Abp.Application.Features;
using Abp.Application.Navigation;
using Abp.Application.Services;
using Abp.Auditing;
using Abp.Authorization;
using Abp.BackgroundJobs;
using Abp.Collections.Extensions;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Abp.Localization;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Net.Mail;
using Abp.Notifications;
using Abp.Runtime.Caching;
using Abp.Runtime.Validation.Interception;
using Abp.Threading;
using Abp.Threading.BackgroundWorkers;
using Abp.Timing;
using Castle.MicroKernel.Registration;

namespace Abp
{
    /// <summary>
    /// Kernel (core) module of the ABP system.No need to depend on this, it's automatically the first module always.
    /// Abp系统的核心模块，不用依赖它，它总是会自动成为第一个模块
    /// </summary>
    public sealed class AbpKernelModule : AbpModule
    {
        /// <summary>
        /// 这是应用启动调用的第一个事件，这里面的代码，会在依赖注入注册之前运行
        /// </summary>
        public override void PreInitialize()
        {
            //注册基本的约定注册器
            IocManager.AddConventionalRegistrar(new BasicConventionalRegistrar());

            //注册IScopedIocResolver
            IocManager.Register<IScopedIocResolver, ScopedIocResolver>(DependencyLifeStyle.Transient);

            //初始化验证拦截器
            ValidationInterceptorRegistrar.Initialize(IocManager);

            //初始化审计拦截器
            AuditingInterceptorRegistrar.Initialize(IocManager);

            //工作单元拦截器初始化
            UnitOfWorkRegistrar.Initialize(IocManager);

            //授权拦截器拦截器
            AuthorizationInterceptorRegistrar.Initialize(IocManager);

            //添加审计的选择器
            Configuration.Auditing.Selectors.Add(
                //应用服务（IApplicationService）需要记录审计日志
                new NamedTypeSelector(
                    "Abp.ApplicationServices",
                    type => typeof(IApplicationService).IsAssignableFrom(type)
                    )
                );

            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    AbpConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(), "Abp.Localization.Sources.AbpXmlSource"
                        )));

            //添加本地化源
            Configuration.Settings.Providers.Add<LocalizationSettingProvider>();
            //添加邮件设置服务
            Configuration.Settings.Providers.Add<EmailSettingProvider>();
            Configuration.Settings.Providers.Add<NotificationSettingProvider>();
            Configuration.Settings.Providers.Add<TimingSettingProvider>();

            Configuration.UnitOfWork.RegisterFilter(AbpDataFilters.SoftDelete, true);
            Configuration.UnitOfWork.RegisterFilter(AbpDataFilters.MustHaveTenant, true);
            Configuration.UnitOfWork.RegisterFilter(AbpDataFilters.MayHaveTenant, true);

            ConfigureCaches();
            AddIgnoredTypes();
        }

        /// <summary>
        /// 这个方法用于模块的依赖注册
        /// </summary>
        public override void Initialize()
        {
            foreach (var replaceAction in ((AbpStartupConfiguration)Configuration).ServiceReplaceActions.Values)
            {
                replaceAction();
            }

            IocManager.IocContainer.Install(new EventBusInstaller(IocManager));

            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly(),
                new ConventionalRegistrationConfig
                {
                    InstallInstallers = false
                });
        }

        /// <summary>
        /// 这个方法在应用启动最后被调用
        /// </summary>
        public override void PostInitialize()
        {
            RegisterMissingComponents();

            IocManager.Resolve<SettingDefinitionManager>().Initialize();
            IocManager.Resolve<FeatureManager>().Initialize();
            IocManager.Resolve<PermissionManager>().Initialize();
            IocManager.Resolve<LocalizationManager>().Initialize();
            IocManager.Resolve<NotificationDefinitionManager>().Initialize();
            IocManager.Resolve<NavigationManager>().Initialize();

            if (Configuration.BackgroundJobs.IsJobExecutionEnabled)
            {
                var workerManager = IocManager.Resolve<IBackgroundWorkerManager>();
                workerManager.Start();
                workerManager.Add(IocManager.Resolve<IBackgroundJobManager>());
            }
        }

        /// <summary>
        /// 这方法，在应用正在关闭时调用
        /// </summary>
        public override void Shutdown()
        {
            if (Configuration.BackgroundJobs.IsJobExecutionEnabled)
            {
                IocManager.Resolve<IBackgroundWorkerManager>().StopAndWaitToStop();
            }
        }

        /// <summary>
        /// 配置缓存
        /// </summary>
        private void ConfigureCaches()
        {
            Configuration.Caching.Configure(AbpCacheNames.ApplicationSettings, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromHours(8);
            });

            Configuration.Caching.Configure(AbpCacheNames.TenantSettings, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(60);
            });

            Configuration.Caching.Configure(AbpCacheNames.UserSettings, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(20);
            });
        }

        /// <summary>
        /// 添加忽略类型
        /// </summary>
        private void AddIgnoredTypes()
        {
            var commonIgnoredTypes = new[] { typeof(Stream), typeof(Expression) };

            foreach (var ignoredType in commonIgnoredTypes)
            {
                Configuration.Auditing.IgnoredTypes.AddIfNotContains(ignoredType);
                Configuration.Validation.IgnoredTypes.AddIfNotContains(ignoredType);
            }

            var validationIgnoredTypes = new[] { typeof(Type) };
            foreach (var ignoredType in validationIgnoredTypes)
            {
                Configuration.Validation.IgnoredTypes.AddIfNotContains(ignoredType);
            }
        }

        /// <summary>
        /// 注册缺失组件
        /// </summary>
        private void RegisterMissingComponents()
        {
            if (!IocManager.IsRegistered<IGuidGenerator>())
            {
                IocManager.IocContainer.Register(
                    Component
                        .For<IGuidGenerator, SequentialGuidGenerator>()
                        .Instance(SequentialGuidGenerator.Instance)
                );
            }

            IocManager.RegisterIfNot<IUnitOfWork, NullUnitOfWork>(DependencyLifeStyle.Transient);
            IocManager.RegisterIfNot<IAuditingStore, SimpleLogAuditingStore>(DependencyLifeStyle.Singleton);
            IocManager.RegisterIfNot<IPermissionChecker, NullPermissionChecker>(DependencyLifeStyle.Singleton);
            IocManager.RegisterIfNot<IRealTimeNotifier, NullRealTimeNotifier>(DependencyLifeStyle.Singleton);
            IocManager.RegisterIfNot<INotificationStore, NullNotificationStore>(DependencyLifeStyle.Singleton);
            IocManager.RegisterIfNot<IUnitOfWorkFilterExecuter, NullUnitOfWorkFilterExecuter>(DependencyLifeStyle.Singleton);
            IocManager.RegisterIfNot<IClientInfoProvider, NullClientInfoProvider>(DependencyLifeStyle.Singleton);

            if (Configuration.BackgroundJobs.IsJobExecutionEnabled)
            {
                IocManager.RegisterIfNot<IBackgroundJobStore, InMemoryBackgroundJobStore>(DependencyLifeStyle.Singleton);
            }
            else
            {
                IocManager.RegisterIfNot<IBackgroundJobStore, NullBackgroundJobStore>(DependencyLifeStyle.Singleton);
            }
        }
    }
}