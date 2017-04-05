using System.Collections.Generic;
using System.Linq;

namespace Abp.Application.Navigation
{
    /// <summary>
    /// <see cref="MenuItemDefinition"/>的扩展方法.
    /// </summary>
    public static class MenuItemDefinitionExtensions
    {
        /// <summary>
        /// 移动一个菜单项到菜单项集合顶部.
        /// </summary>
        /// <param name="menuItems">菜单项列表</param>
        /// <param name="menuItemName">被移动的菜单项名称</param>
        public static void MoveMenuItemToTop(this IList<MenuItemDefinition> menuItems, string menuItemName)
        {
            var menuItem = GetMenuItem(menuItems, menuItemName);
            menuItems.Remove(menuItem);
            menuItems.Insert(0, menuItem);
        }

        /// <summary>
        /// 移动一个菜单项到菜单项集合底部.
        /// </summary>
        /// <param name="menuItems">菜单项列表</param>
        /// <param name="menuItemName">被移动的菜单项名称</param>
        public static void MoveMenuItemToBottom(this IList<MenuItemDefinition> menuItems, string menuItemName)
        {
            var menuItem = GetMenuItem(menuItems, menuItemName);
            menuItems.Remove(menuItem);
            menuItems.Insert(menuItems.Count, menuItem);
        }

        /// <summary>
        /// 移动一个菜单项到另一个菜单项后面.
        /// </summary>
        /// <param name="menuItems">菜单项列表</param>
        /// <param name="menuItemName">需移动的菜单项名称</param>
        /// <param name="targetMenuItemName">目标菜单项名称</param>
        public static void MoveMenuItemBefore(this IList<MenuItemDefinition> menuItems, string menuItemName, string targetMenuItemName)
        {
            var menuItem = GetMenuItem(menuItems, menuItemName);
            var targetMenuItem = GetMenuItem(menuItems, targetMenuItemName);
            menuItems.Remove(menuItem);
            menuItems.Insert(menuItems.IndexOf(targetMenuItem), menuItem);
        }

        /// <summary>
        /// 移动一个菜单项到另一个菜单项前面.
        /// </summary>
        /// <param name="menuItems">菜单项列表</param>
        /// <param name="menuItemName">需移动的菜单项名称</param>
        /// <param name="targetMenuItemName">目标菜单项名称</param>
        public static void MoveMenuItemAfter(this IList<MenuItemDefinition> menuItems, string menuItemName, string targetMenuItemName)
        {
            var menuItem = GetMenuItem(menuItems, menuItemName);
            var targetMenuItem = GetMenuItem(menuItems, targetMenuItemName);
            menuItems.Remove(menuItem);
            menuItems.Insert(menuItems.IndexOf(targetMenuItem) + 1, menuItem);
        }

        /// <summary>
        /// 获取菜单项
        /// </summary>
        /// <param name="menuItems">菜单项列表</param>
        /// <param name="menuItemName">菜单项名称</param>
        /// <returns></returns>
        private static MenuItemDefinition GetMenuItem(IEnumerable<MenuItemDefinition> menuItems, string menuItemName)
        {
            var menuItem = menuItems.FirstOrDefault(i => i.Name == menuItemName);
            if (menuItem == null)
            {
                throw new AbpException("Can not find menu item: " + menuItemName);
            }

            return menuItem;
        }
    }
}