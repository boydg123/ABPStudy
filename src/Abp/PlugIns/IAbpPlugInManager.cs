namespace Abp.PlugIns
{
    /// <summary>
    /// ABP插件管理器
    /// </summary>
    public interface IAbpPlugInManager
    {
        /// <summary>
        /// 插件源列表
        /// </summary>
        PlugInSourceList PlugInSources { get; }
    }
}
