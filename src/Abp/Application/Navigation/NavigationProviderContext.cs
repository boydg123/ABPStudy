namespace Abp.Application.Navigation
{
    /// <summary>
    /// 导航提供者上下文
    /// </summary>
    internal class NavigationProviderContext : INavigationProviderContext
    {
        /// <summary>
        /// 导航管理接口
        /// </summary>
        public INavigationManager Manager { get; private set; }

        /// <summary>
        /// 初始化 <see cref="NavigationProviderContext"/>
        /// </summary>
        /// <param name="manager">导航管理接口</param>
        public NavigationProviderContext(INavigationManager manager)
        {
            Manager = manager;
        }
    }
}