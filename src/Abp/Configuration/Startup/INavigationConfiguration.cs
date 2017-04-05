using Abp.Application.Navigation;
using Abp.Collections;

namespace Abp.Configuration.Startup
{
    /// <summary>
    /// Used to configure navigation.
    /// 用来配置一个导航
    /// </summary>
    public interface INavigationConfiguration
    {
        /// <summary>
        /// List of navigation providers.
        /// 导航驱动类列表
        /// </summary>
        ITypeList<NavigationProvider> Providers { get; }
    }
}