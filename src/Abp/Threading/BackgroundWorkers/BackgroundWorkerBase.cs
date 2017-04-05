using System.Globalization;
using Abp.Configuration;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Localization.Sources;
using Castle.Core.Logging;

namespace Abp.Threading.BackgroundWorkers
{
    /// <summary>
    /// Base class that can be used to implement <see cref="IBackgroundWorker"/>.
    /// 用于实现<see cref="IBackgroundWorker"/>的基类
    /// </summary>
    public abstract class BackgroundWorkerBase : RunnableBase, IBackgroundWorker
    {
        /// <summary>
        /// Reference to the setting manager.
        /// 设置管理的一个引用
        /// </summary>
        public ISettingManager SettingManager { protected get; set; }

        /// <summary>
        /// Reference to <see cref="IUnitOfWorkManager"/>.
        /// <see cref="IUnitOfWorkManager"/>的引用
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
        /// 获取当前工作单元
        /// </summary>
        protected IActiveUnitOfWork CurrentUnitOfWork { get { return UnitOfWorkManager.Current; } }

        /// <summary>
        /// Reference to the localization manager.
        /// 本地化管理的引用
        /// </summary>
        public ILocalizationManager LocalizationManager { protected get; set; }

        /// <summary>
        /// Gets/sets name of the localization source that is used in this application service.
        /// 获取/设置当前应用程序服务本地化资源的名称
        /// It must be set in order to use <see cref="L(string)"/> and <see cref="L(string,CultureInfo)"/> methods.
        /// 它必须被设置用<see cref="L(string)"/>和<see cref="L(string,CultureInfo)"/>方法排序
        /// </summary>
        protected string LocalizationSourceName { get; set; }

        /// <summary>
        /// Gets localization source.It's valid if <see cref="LocalizationSourceName"/> is set.
        /// 获取本地化资源。如果<see cref="LocalizationSourceName"/>被设置。
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
        /// 写log的日志引用
        /// </summary>
        public ILogger Logger { protected get; set; }

        /// <summary>
        /// Constructor.
        /// 构造函数
        /// </summary>
        protected BackgroundWorkerBase()
        {
            Logger = NullLogger.Instance;
            LocalizationManager = NullLocalizationManager.Instance;
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public override void Start()
        {
            base.Start();
            Logger.Debug("Start background worker: " + ToString());
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public override void Stop()
        {
            base.Stop();
            Logger.Debug("Stop background worker: " + ToString());
        }

        /// <summary>
        /// 等待服务停止
        /// </summary>
        public override void WaitToStop()
        {
            base.WaitToStop();
            Logger.Debug("WaitToStop background worker: " + ToString());
        }

        /// <summary>
        /// Gets localized string for given key name and current language.
        /// 根据给定的当前语言key获取本地化字符串
        /// </summary>
        /// <param name="name">Key name / key</param>
        /// <returns>Localized string / 本地化字符串</returns>
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
        /// 获取具有给定字符串的给定键名称和当前语言的本地化字符串
        /// </summary>
        /// <param name="name">Key name / key</param>
        /// <param name="culture">culture information / culture信息</param>
        /// <param name="args">Format arguments / 格式化参数</param>
        /// <returns>Localized string / 本地化字符串</returns>
        protected string L(string name, CultureInfo culture, params object[] args)
        {
            return LocalizationSource.GetString(name, culture, args);
        }

        /// <summary>
        /// 重写ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GetType().FullName;
        }
    }
}