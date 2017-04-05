using Abp.Application.Navigation;
using Abp.Collections;

namespace Abp.Configuration.Startup
{
    /// <summary>
    /// 用来配置一个导航
    /// </summary>
    public class NavigationConfiguration : INavigationConfiguration
    {
        /// <summary>
        /// 导航驱动类列表
        /// </summary>
        public ITypeList<NavigationProvider> Providers { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public NavigationConfiguration()
        {
            Providers = new TypeList<NavigationProvider>();
        }
    }
}