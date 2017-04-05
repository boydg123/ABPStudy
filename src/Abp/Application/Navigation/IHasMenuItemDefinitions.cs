using System.Collections.Generic;

namespace Abp.Application.Navigation
{
    /// <summary>
    /// 声明这些菜单项类的公用接口.
    /// </summary>
    public interface IHasMenuItemDefinitions
    {
        /// <summary>
        /// 菜单项列表.
        /// </summary>
        IList<MenuItemDefinition> Items { get; }
    }
}