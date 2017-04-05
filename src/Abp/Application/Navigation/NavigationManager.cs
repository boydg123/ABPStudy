using System.Collections.Generic;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Localization;

namespace Abp.Application.Navigation
{
    /// <summary>
    /// 导航管理类
    /// </summary>
    public class NavigationManager : INavigationManager, ISingletonDependency
    {
        /// <summary>
        /// 菜单字典
        /// </summary>
        public IDictionary<string, MenuDefinition> Menus { get; private set; }

        /// <summary>
        /// 主菜单
        /// </summary>
        public MenuDefinition MainMenu
        {
            get { return Menus["MainMenu"]; }
        }

        /// <summary>
        /// 解决依赖关系的IOC接口
        /// </summary>
        private readonly IIocResolver _iocResolver;
        /// <summary>
        /// 导航配置接口
        /// </summary>
        private readonly INavigationConfiguration _configuration;

        /// <summary>
        /// 构造函数：初始化当前导航
        /// </summary>
        /// <param name="iocResolver">IOC接口</param>
        /// <param name="configuration">导航配置接口</param>
        public NavigationManager(IIocResolver iocResolver, INavigationConfiguration configuration)
        {
            _iocResolver = iocResolver;
            _configuration = configuration;

            Menus = new Dictionary<string, MenuDefinition>
                    {
                        {"MainMenu", new MenuDefinition("MainMenu", new FixedLocalizableString("Main menu"))} //TODO: Localization for "Main menu"
                    };
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            var context = new NavigationProviderContext(this);

            foreach (var providerType in _configuration.Providers)
            {
                using (var provider = _iocResolver.ResolveAsDisposable<NavigationProvider>(providerType))
                {
                    provider.Object.SetNavigation(context);
                }
            }
        }
    }
}