using Abp.Dependency;

namespace Abp.Localization
{
    /// <summary>
    /// Implements <see cref="ILocalizationContext"/>.
    /// <see cref="ILocalizationContext"/>的实现
    /// </summary>
    public class LocalizationContext : ILocalizationContext, ISingletonDependency
    {
        /// <summary>
        /// 本地化管理器
        /// </summary>
        public ILocalizationManager LocalizationManager { get; private set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationContext"/> class.
        /// 构造函数
        /// </summary>
        /// <param name="localizationManager">The localization manager. / 本地化管理器</param>
        public LocalizationContext(ILocalizationManager localizationManager)
        {
            LocalizationManager = localizationManager;
        }
    }
}