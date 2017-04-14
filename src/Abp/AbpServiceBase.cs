using System.Globalization;
using Abp.Configuration;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.ObjectMapping;
using Castle.Core.Logging;

namespace Abp
{
    /// <summary>
    /// This class can be used as a base class for services.
    /// It has some useful objects property-injected and has some basic methods most of services may need to.
    /// 此类能被用作服务的基类
    /// 它有一些有用的属性注入的对象和一些大多数服务需要的基本的方法
    /// </summary>
    public abstract class AbpServiceBase
    {
        /// <summary>
        /// Reference to the setting manager.
        /// 设置管理器引用
        /// </summary>
        public ISettingManager SettingManager { get; set; }

        /// <summary>
        /// Reference to <see cref="IUnitOfWorkManager"/>.
        /// 工作单元管理器<see cref="IUnitOfWorkManager"/>引用
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
        /// Reference to the localization manager.
        /// 本地化管理器引用
        /// </summary>
        public ILocalizationManager LocalizationManager { get; set; }

        /// <summary>
        /// Gets/sets name of the localization source that is used in this application service.
        /// 获取/设置用于此应用服务的本地化源名称
        /// It must be set in order to use <see cref="L(string)"/> and <see cref="L(string,CultureInfo)"/> methods.
        /// 它必须设置，以便使用方法<see cref="L(string)"/> 和 <see cref="L(string,CultureInfo)"/>
        /// </summary>
        protected string LocalizationSourceName { get; set; }

        /// <summary>
        /// Gets localization source.
        /// 获取本地化源
        /// It's valid if <see cref="LocalizationSourceName"/> is set.
        /// 如果<see cref="LocalizationSourceName"/>被设置，它才有效
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
        /// 记录日志的日志器引用
        /// </summary>
        public ILogger Logger { protected get; set; }

        /// <summary>
        /// Reference to the object to object mapper.
        /// 引用对象映射器
        /// </summary>
        public IObjectMapper ObjectMapper { get; set; }

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        protected AbpServiceBase()
        {
            Logger = NullLogger.Instance;
            ObjectMapper = NullObjectMapper.Instance;
            LocalizationManager = NullLocalizationManager.Instance;
        }

        /// <summary>
        /// Gets localized string for given key name and current language.
        /// 获取给定名称和当前语言的本地化字符串
        /// </summary>
        /// <param name="name">Key name / 键名</param>
        /// <returns>Localized string / 本地化字符串</returns>
        protected virtual string L(string name)
        {
            return LocalizationSource.GetString(name);
        }

        /// <summary>
        /// Gets localized string for given key name and current language with formatting strings.
        /// 获取给定名称和当前语言的格式化后的本地化字符串
        /// </summary>
        /// <param name="name">Key name / 键名</param>
        /// <param name="args">Format arguments / 格式化字符串</param>
        /// <returns>Localized string / 本地化字符串</returns>
        protected string L(string name, params object[] args)
        {
            return LocalizationSource.GetString(name, args);
        }

        /// <summary>
        /// Gets localized string for given key name and specified culture information.
        /// 获取给定名称和给定区域区域的格式化后的本地化字符串
        /// </summary>
        /// <param name="name">Key name / 键名</param>
        /// <param name="culture">culture information / 区域区域信息</param>
        /// <returns>Localized string / 本地化字符串</returns>
        protected virtual string L(string name, CultureInfo culture)
        {
            return LocalizationSource.GetString(name, culture);
        }

        /// <summary>
        /// Gets localized string for given key name and current language with formatting strings.
        /// 获取给定名称和给定区域区域的格式化后的格式化后的本地化字符串
        /// </summary>
        /// <param name="name">Key name / 键名</param>
        /// <param name="culture">culture information / 区域区域信息</param>
        /// <param name="args">Format arguments / 格式化参数</param>
        /// <returns>Localized string / 本地化字符串</returns>
        protected string L(string name, CultureInfo culture, params object[] args)
        {
            return LocalizationSource.GetString(name, culture, args);
        }
    }
}