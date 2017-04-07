using System.Globalization;
using Abp.Configuration;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.ObjectMapping;
using Castle.Core.Logging;
using Quartz;

namespace Abp.Quartz.Quartz
{
    /// <summary>
    /// 作业基类
    /// </summary>
    public abstract class JobBase : IJob
    {
        /// <summary>
        /// 本地资源引用
        /// </summary>
        private ILocalizationSource _localizationSource;

        /// <summary>
        /// 工作单元管理器
        /// </summary>
        private IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        protected JobBase()
        {
            Logger = NullLogger.Instance;
            LocalizationManager = NullLocalizationManager.Instance;
        }

        /// <summary>
        /// 日志记录器
        /// </summary>
        public ILogger Logger { protected get; set; }

        /// <summary>
        /// Reference to the setting manager.
        /// 设置管理引用
        /// </summary>
        public ISettingManager SettingManager { get; set; }

        /// <summary>
        /// Reference to <see cref="IUnitOfWorkManager" />.
        /// <see cref="IUnitOfWorkManager" />引用
        /// </summary>
        public IUnitOfWorkManager UnitOfWorkManager
        {
            get
            {
                if (_unitOfWorkManager == null)
                    throw new AbpException("Must set UnitOfWorkManager before use it.");

                return _unitOfWorkManager;
            }
            set { _unitOfWorkManager = value; }
        }

        /// <summary>
        /// Gets current unit of work.
        /// 获取当前工作单元
        /// </summary>
        protected IActiveUnitOfWork CurrentUnitOfWork
        {
            get { return UnitOfWorkManager.Current; }
        }

        /// <summary>
        /// Reference to the localization manager.
        /// 本地化管理器引用
        /// </summary>
        public ILocalizationManager LocalizationManager { get; set; }

        /// <summary>
        /// Gets/sets name of the localization source that is used in this application service.
        /// 获取/设置当前应用程序服务本地化资源的名称
        /// It must be set in order to use <see cref="L(string)" /> and <see cref="L(string,CultureInfo)" /> methods.
        /// 它必须被设置用<see cref="L(string)"/>和<see cref="L(string,CultureInfo)"/>方法排序
        /// </summary>
        protected string LocalizationSourceName { get; set; }

        /// <summary>
        /// Gets localization source.It's valid if <see cref="LocalizationSourceName" /> is set.
        /// 获取本地化资源。如果<see cref="LocalizationSourceName"/>被设置。    
        /// </summary>
        protected ILocalizationSource LocalizationSource
        {
            get
            {
                if (LocalizationSourceName == null)
                    throw new AbpException("Must set LocalizationSourceName before, in order to get LocalizationSource");

                if ((_localizationSource == null) || (_localizationSource.Name != LocalizationSourceName))
                    _localizationSource = LocalizationManager.GetSource(LocalizationSourceName);

                return _localizationSource;
            }
        }

        /// <summary>
        /// Reference to the object to object mapper.
        /// 对象映射器接口
        /// </summary>
        public IObjectMapper ObjectMapper { get; set; }

        /// <summary>
        /// 执行作业
        /// </summary>
        /// <param name="context">作业执行器上下文</param>
        public abstract void Execute(IJobExecutionContext context);

        /// <summary>
        /// Gets localized string for given key name and current language.
        /// 在当前语言环境下通过key名称获取本地字符串
        /// </summary>
        /// <param name="name">Key name / Key名称</param>
        /// <returns>Localized string / 本地字符串</returns>
        protected virtual string L(string name)
        {
            return LocalizationSource.GetString(name);
        }

        /// <summary>
        /// Gets localized string for given key name and current language with formatting strings.
        /// 获取本地化的字符串给定键的名称和当前语言格式化字符串
        /// </summary>
        /// <param name="name">Key name / key</param>
        /// <param name="args">Format arguments / 格式化参数</param>
        /// <returns>Localized string / 本地化字符串</returns>
        protected string L(string name, params object[] args)
        {
            return LocalizationSource.GetString(name, args);
        }

        /// <summary>
        /// Gets localized string for given key name and specified culture information.
        /// 获取给定键名称和指定的文化信息的本地化字符串
        /// </summary>
        /// <param name="name">Key name / key</param>
        /// <param name="culture">culture information / culture信息</param>
        /// <returns>Localized string / 本地化字符串</returns>
        protected virtual string L(string name, CultureInfo culture)
        {
            return LocalizationSource.GetString(name, culture);
        }

        /// <summary>
        /// Gets localized string for given key name and current language with formatting strings.
        /// 获取本地化的字符串给定键的名称和当前语言格式化字符串
        /// </summary>
        /// <param name="name">Key name / key</param>
        /// <param name="culture">culture information / culture信息</param>
        /// <param name="args">Format arguments / 格式化参数</param>
        /// <returns>Localized string / 本地化字符串</returns>
        protected string L(string name, CultureInfo culture, params object[] args)
        {
            return LocalizationSource.GetString(name, culture, args);
        }
    }
}