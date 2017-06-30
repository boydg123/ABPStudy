using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Features;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.Runtime.Session;
using Castle.Core.Logging;

namespace Abp.WebApi.Controllers
{
    /// <summary>
    /// Base class for all ApiControllers in web applications those use Abp system.
    /// ABP系统web应用程序所有ApiControllers的基类
    /// </summary>
    public abstract class AbpApiController : ApiController
    {
        /// <summary>
        /// Gets current session information.
        /// Abp Session引用
        /// </summary>
        public IAbpSession AbpSession { get; set; }

        /// <summary>
        /// Gets the event bus.
        /// Event Bus引用
        /// </summary>
        public IEventBus EventBus { get; set; }

        /// <summary>
        /// Reference to the permission manager.
        /// 权限管理引用
        /// </summary>
        public IPermissionManager PermissionManager { get; set; }

        /// <summary>
        /// Reference to the setting manager.
        /// 设置管理引用
        /// </summary>
        public ISettingManager SettingManager { get; set; }

        /// <summary>
        /// Reference to the permission checker.
        /// 权限检查器引用
        /// </summary>
        public IPermissionChecker PermissionChecker { protected get; set; }

        /// <summary>
        /// Reference to the feature manager.
        /// 功能管理引用
        /// </summary>
        public IFeatureManager FeatureManager { protected get; set; }

        /// <summary>
        /// Reference to the permission checker.
        /// 功能检查器引用
        /// </summary>
        public IFeatureChecker FeatureChecker { protected get; set; }

        /// <summary>
        /// Reference to the localization manager.
        /// 本地化管理引用
        /// </summary>
        public ILocalizationManager LocalizationManager { protected get; set; }

        /// <summary>
        /// Gets/sets name of the localization source that is used in this application service.
        /// 在当前应用程序服务获取本地化资源的名称
        /// It must be set in order to use <see cref="L(string)"/> and <see cref="L(string,CultureInfo)"/> methods.
        /// 它必须使用<see cref="L(string)"/> and <see cref="L(string,CultureInfo)"/> 方法排序
        /// </summary>
        protected string LocalizationSourceName { get; set; }

        /// <summary>
        /// Gets localization source.
        /// 获取本地源
        /// It's valid if <see cref="LocalizationSourceName"/> is set.
        /// 如果<see cref="LocalizationSourceName"/>是设置的，则它是有效的
        /// </summary>
        protected ILocalizationSource LocalizationSource
        {
            get
            {
                if (LocalizationSourceName == null)
                {
                    throw new AbpException("Must set LocalizationSourceName before, in order to get LocalizationSource");
                }

                if (_localizationSource == null || _localizationSource.Name != LocalizationSourceName)
                {
                    _localizationSource = LocalizationManager.GetSource(LocalizationSourceName);
                }

                return _localizationSource;
            }
        }
        private ILocalizationSource _localizationSource;

        /// <summary>
        /// Reference to the logger to write logs.
        /// 日志引用
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Reference to <see cref="IUnitOfWorkManager"/>.
        /// 工作单元引用
        /// </summary>
        public IUnitOfWorkManager UnitOfWorkManager
        {
            get
            {
                if (_unitOfWorkManager == null)
                {
                    throw new AbpException("Must set UnitOfWorkManager before use it.");
                }

                return _unitOfWorkManager;
            }
            set { _unitOfWorkManager = value; }
        }
        private IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// Gets current unit of work.
        /// 获取当前的工作单元
        /// </summary>
        protected IActiveUnitOfWork CurrentUnitOfWork { get { return UnitOfWorkManager.Current; } }

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        protected AbpApiController()
        {
            AbpSession = NullAbpSession.Instance;
            Logger = NullLogger.Instance;
            LocalizationManager = NullLocalizationManager.Instance;
            PermissionChecker = NullPermissionChecker.Instance;
            EventBus = NullEventBus.Instance;
        }
        
        /// <summary>
        /// Gets localized string for given key name and current language.
        /// 通过给定的name和当前语言获取本地化字符串
        /// </summary>
        /// <param name="name">Key name / key</param>
        /// <returns>Localized string / 对应的字符串</returns>
        protected virtual string L(string name)
        {
            return LocalizationSource.GetString(name);
        }

        /// <summary>
        /// Gets localized string for given key name and current language with formatting strings.
        /// 通过给定的key和当前语言获取格式化的本地化字符串
        /// </summary>
        /// <param name="name">Key name / Key</param>
        /// <param name="args">Format arguments / 格式化参数</param>
        /// <returns>Localized string / 本地化字符串</returns>
        protected string L(string name, params object[] args)
        {
            return LocalizationSource.GetString(name, args);
        }

        /// <summary>
        /// Gets localized string for given key name and specified culture information.
        /// 通过给定的Key和指定的区域信息获取本地化字符串
        /// </summary>
        /// <param name="name">Key name / Key</param>
        /// <param name="culture">culture information / 区域信息</param>
        /// <returns>Localized string / 本地化字符串</returns>
        protected virtual string L(string name, CultureInfo culture)
        {
            return LocalizationSource.GetString(name, culture);
        }

        /// <summary>
        /// Gets localized string for given key name and current language with formatting strings.
        /// 通过给定的Key和当前语言获取格式化的本地化字符串
        /// </summary>
        /// <param name="name">Key name / Key</param>
        /// <param name="culture">culture information / 区域信息</param>
        /// <param name="args">Format arguments / 格式化参数</param>
        /// <returns>Localized string / 本地化字符串</returns>
        protected string L(string name, CultureInfo culture, params object[] args)
        {
            return LocalizationSource.GetString(name, culture, args);
        }

        /// <summary>
        /// Checks if current user is granted for a permission.
        /// 检查当前用户是否被授予权限
        /// </summary>
        /// <param name="permissionName">Name of the permission / 权限名称</param>
        protected Task<bool> IsGrantedAsync(string permissionName)
        {
            return PermissionChecker.IsGrantedAsync(permissionName);
        }

        /// <summary>
        /// Checks if current user is granted for a permission.
        /// 检查当前用户是否被授予权限
        /// </summary>
        /// <param name="permissionName">Name of the permission / 权限名称</param>
        protected bool IsGranted(string permissionName)
        {
            return PermissionChecker.IsGranted(permissionName);
        }
        
        /// <summary>
        /// Checks if given feature is enabled for current tenant.
        /// 检查当前功能是否对当前租户启用
        /// </summary>
        /// <param name="featureName">Name of the feature / 功能名称</param>
        /// <returns></returns>
        protected virtual Task<bool> IsEnabledAsync(string featureName)
        {
            return FeatureChecker.IsEnabledAsync(featureName);
        }

        /// <summary>
        /// Checks if given feature is enabled for current tenant.
        /// 检查当前功能是否对当前租户启用
        /// </summary>
        /// <param name="featureName">Name of the feature / 功能名称</param>
        /// <returns></returns>
        protected virtual bool IsEnabled(string featureName)
        {
            return FeatureChecker.IsEnabled(featureName);
        }
    }
}
