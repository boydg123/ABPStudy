using System.Collections.Generic;

namespace Abp.Application.Navigation
{
    /// <summary>
    /// 应用中导航的管理类接口.
    /// </summary>
    public interface INavigationManager
    {
        /// <summary>
        /// 当前应用程序所有菜单.
        /// </summary>
        IDictionary<string, MenuDefinition> Menus { get; }

        /// <summary>
        /// 获取当前应用程序的主菜单.
        /// <see cref="Menus"/>的快捷方式["MainMenu"].
        /// </summary>
        MenuDefinition MainMenu { get; }
    }
}
