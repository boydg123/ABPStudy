using System;
using System.Collections.Generic;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Abp.Notifications;
using Abp.Runtime.Caching.Configuration;

namespace Abp.Configuration.Startup
{
    /// <summary>
    /// This class is used to configure ABP and modules on startup.
    /// 这个类是在启动时配置ABP和模块
    /// </summary>
    internal class AbpStartupConfiguration : DictionaryBasedConfig, IAbpStartupConfiguration
    {
        /// <summary>
        /// Reference to the IocManager.
        /// IOC管理器引用
        /// </summary>
        public IIocManager IocManager { get; }

        /// <summary>
        /// Used to set localization configuration.
        /// 用于设置本地化配置
        /// </summary>
        public ILocalizationConfiguration Localization { get; private set; }

        /// <summary>
        /// Used to configure authorization.
        /// 用于配置授权
        /// </summary>
        public IAuthorizationConfiguration Authorization { get; private set; }

        /// <summary>
        /// Used to configure validation.
        /// 用于配置验证
        /// </summary>
        public IValidationConfiguration Validation { get; private set; }

        /// <summary>
        /// Used to configure settings.
        /// 用于配置settings
        /// </summary>
        public ISettingsConfiguration Settings { get; private set; }

        /// <summary>
        /// Gets/sets default connection string used by ORM module.It can be name of a connection string in application's config file or can be full connection string.
        /// 获取/设置ORM模块使用的默认连接字符串，它可以是一个完整的连接字符串，也可以是应用程序配置文件中的连接字符串名称
        /// </summary>
        public string DefaultNameOrConnectionString { get; set; }

        /// <summary>
        /// Used to configure modules.Modules can write extension methods to <see cref="ModuleConfigurations"/> to add module specific configurations.
        /// 用于配置模块，模块可以写 <see cref="ModuleConfigurations"/> 的扩展方法，来添加模块配置
        /// </summary>
        public IModuleConfigurations Modules { get; private set; }

        /// <summary>
        /// Used to configure unit of work defaults.
        /// 用于配置工作单元
        /// </summary>
        public IUnitOfWorkDefaultOptions UnitOfWork { get; private set; }

        /// <summary>
        /// Used to configure features.
        /// 用于配置功能
        /// </summary>
        public IFeatureConfiguration Features { get; private set; }

        /// <summary>
        /// Used to configure background job system.
        /// 用于配置后台作业系统
        /// </summary>
        public IBackgroundJobConfiguration BackgroundJobs { get; private set; }

        /// <summary>
        /// Used to configure notification system.
        /// 用于配置通知系统
        /// </summary>
        public INotificationConfiguration Notifications { get; private set; }

        /// <summary>
        /// Used to configure navigation.
        /// 用于配置导航属性（菜单）
        /// </summary>
        public INavigationConfiguration Navigation { get; private set; }

        /// <summary>
        /// Used to configure <see cref="IEventBus"/>.
        /// 用于配置<see cref="IEventBus"/>.
        /// </summary>
        public IEventBusConfiguration EventBus { get; private set; }

        /// <summary>
        /// Used to configure auditing.
        /// 用于配置审计
        /// </summary>
        public IAuditingConfiguration Auditing { get; private set; }

        /// <summary>
        /// 用于配置缓存
        /// </summary>
        public ICachingConfiguration Caching { get; private set; }

        /// <summary>
        /// Used to configure multi-tenancy.
        /// 用于配置多租户
        /// </summary>
        public IMultiTenancyConfig MultiTenancy { get; private set; }

        /// <summary>
        /// 服务替换方法字典
        /// </summary>
        public Dictionary<Type, Action> ServiceReplaceActions { get; private set; }

        /// <summary>
        /// Private constructor for singleton pattern.
        /// 单例模式私有构造函数
        /// </summary>
        public AbpStartupConfiguration(IIocManager iocManager)
        {
            IocManager = iocManager;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            Localization = IocManager.Resolve<ILocalizationConfiguration>();
            Modules = IocManager.Resolve<IModuleConfigurations>();
            Features = IocManager.Resolve<IFeatureConfiguration>();
            Navigation = IocManager.Resolve<INavigationConfiguration>();
            Authorization = IocManager.Resolve<IAuthorizationConfiguration>();
            Validation = IocManager.Resolve<IValidationConfiguration>();
            Settings = IocManager.Resolve<ISettingsConfiguration>();
            UnitOfWork = IocManager.Resolve<IUnitOfWorkDefaultOptions>();
            EventBus = IocManager.Resolve<IEventBusConfiguration>();
            MultiTenancy = IocManager.Resolve<IMultiTenancyConfig>();
            Auditing = IocManager.Resolve<IAuditingConfiguration>();
            Caching = IocManager.Resolve<ICachingConfiguration>();
            BackgroundJobs = IocManager.Resolve<IBackgroundJobConfiguration>();
            Notifications = IocManager.Resolve<INotificationConfiguration>();
            ServiceReplaceActions = new Dictionary<Type, Action>();
        }

        /// <summary>
        /// 替换服务
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="replaceAction">替换方法</param>
        public void ReplaceService(Type type, Action replaceAction)
        {
            ServiceReplaceActions[type] = replaceAction;
        }

        /// <summary>
        /// 获取<see cref="T"/>的配置对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>()
        {
            return GetOrCreate(typeof(T).FullName, () => IocManager.Resolve<T>());
        }
    }
}