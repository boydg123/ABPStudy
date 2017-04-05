namespace Abp.Application.Navigation
{
    /// <summary>
    /// 设置导航的基础设施驱动接口.
    /// </summary>
    public interface INavigationProviderContext
    {
        /// <summary>
        /// 获取菜单管理类的一个引用.
        /// </summary>
        INavigationManager Manager { get; }
    }
}