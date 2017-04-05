using System.Collections.Generic;

namespace Abp.PlugIns
{
    /// <summary>
    /// ABP插件管理
    /// </summary>
    public class AbpPlugInManager : IAbpPlugInManager
    {
        /// <summary>
        /// 插件源列表
        /// </summary>
        public PlugInSourceList PlugInSources { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AbpPlugInManager()
        {
            PlugInSources = new PlugInSourceList();
        }
    }
}