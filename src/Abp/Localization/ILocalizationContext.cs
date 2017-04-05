namespace Abp.Localization
{
    /// <summary>
    /// Localization context.
    /// 本地化上下文
    /// </summary>
    public interface ILocalizationContext
    {
        /// <summary>
        /// Gets the localization manager.
        /// 获取本地化管理器
        /// </summary>
        ILocalizationManager LocalizationManager { get; }
    }
}