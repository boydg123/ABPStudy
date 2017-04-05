using System;
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
    /// Used to configure ABP and modules on startup.
    /// 系统启动时用来配置ABP和模块
    /// </summary>
    public interface IAbpStartupConfiguration : IDictionaryBasedConfig
    {
        /// <summary>
        /// Gets the IOC manager associated with this configuration.
        /// 获取与该配置有关的IocManager
        /// </summary>
        IIocManager IocManager { get; }

        /// <summary>
        /// Used to set localization configuration.
        /// 用于设置本地化配置
        /// </summary>
        ILocalizationConfiguration Localization { get; }

        /// <summary>
        /// Used to configure navigation.
        /// 用于配置导航
        /// </summary>
        INavigationConfiguration Navigation { get; }

        /// <summary>
        /// Used to configure <see cref="IEventBus"/>.
        /// 用于配置<see cref="IEventBus"/>.
        /// </summary>
        IEventBusConfiguration EventBus { get; }

        /// <summary>
        /// Used to configure auditing.
        /// 用于审计配置
        /// </summary>
        IAuditingConfiguration Auditing { get; }

        /// <summary>
        /// Used to configure caching.
        /// 用于配置缓存
        /// </summary>
        ICachingConfiguration Caching { get; }

        /// <summary>
        /// Used to configure multi-tenancy.
        /// 用于配置多租户
        /// </summary>
        IMultiTenancyConfig MultiTenancy { get; }

        /// <summary>
        /// Used to configure authorization.
        /// 用于配置授权
        /// </summary>
        IAuthorizationConfiguration Authorization { get; }

        /// <summary>
        /// Used to configure validation.
        /// 用于配置验证
        /// </summary>
        IValidationConfiguration Validation { get; }

        /// <summary>
        /// Used to configure settings.
        /// 用于配置设置(setting)
        /// </summary>
        ISettingsConfiguration Settings { get; }

        /// <summary>
        /// Gets/sets default connection string used by ORM module.It can be name of a connection string in application's config file or can be full connection string.
        /// 获取或设置ORM模块的默认连接字符串,它可以是一个完整的连接字符串，也可以是应用程序配置文件中的连接字符串名称
        /// </summary>
        string DefaultNameOrConnectionString { get; set; }

        /// <summary>
        /// Used to configure modules.Modules can write extension methods to <see cref="IModuleConfigurations"/> to add module specific configurations.
        /// 用于配置模块,模块可以写 <see cref="ModuleConfigurations"/> 的扩展方法，来添加模块配置
        /// </summary>
        IModuleConfigurations Modules { get; }

        /// <summary>
        /// Used to configure unit of work defaults.
        /// 用于配置默认的工作单元
        /// </summary>
        IUnitOfWorkDefaultOptions UnitOfWork { get; }

        /// <summary>
        /// Used to configure features.
        /// 用于配置功能
        /// </summary>
        IFeatureConfiguration Features { get; }

        /// <summary>
        /// Used to configure background job system.
        /// 用于配置后台作业系统
        /// </summary>
        IBackgroundJobConfiguration BackgroundJobs { get; }

        /// <summary>
        /// Used to configure notification system.
        /// 用于配置通知系统
        /// </summary>
        INotificationConfiguration Notifications { get; }

        /// <summary>
        /// Used to replace a service type.Given <see cref="replaceAction"/> should register an implementation for the <see cref="type"/>.
        /// 用于替换服务类型，给定的<see cref="replaceAction"/>应该为<see cref="type"/>注册一个实现
        /// </summary>
        /// <param name="type">The type to be replaced. / 要被替换的类型</param>
        /// <param name="replaceAction">Replace action. / 替换方法</param>
        void ReplaceService(Type type, Action replaceAction);

        /// <summary>
        /// Gets a configuration object.
        /// 获取一个配置对象
        /// </summary>
        T Get<T>();
    }
}