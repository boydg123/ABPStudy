using System.Collections.Generic;
using Abp.Localization;

namespace Abp.Application.Navigation
{
    /// <summary>
    /// Represents an item in a <see cref="UserMenu"/>.
    /// 表示<see cref="UserMenu"/>的一个菜单项
    /// </summary>
    public class UserMenuItem
    {
        /// <summary>
        /// Unique name of the menu item in the application. 
        /// 菜单项名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Icon of the menu item if exists.
        /// 菜单项图标(如果存在)
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Display name of the menu item.
        /// 菜单项显示名
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// The Display order of the menu. Optional.
        /// 菜单项显示序号[选填]
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// The URL to navigate when this menu item is selected.
        /// 当菜单项选中时导航的URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// A custom object related to this menu item.
        /// 菜单项关联的自定义数据
        /// </summary>
        public object CustomData { get; set; }

        /// <summary>
        /// Sub items of this menu item.
        /// 菜单项的子列表
        /// </summary>
        public IList<UserMenuItem> Items { get; private set; }

        /// <summary>
        /// Creates a new <see cref="UserMenuItem"/> object.
        /// 创建一个新的<see cref="UserMenuItem"/>对象
        /// </summary>
        public UserMenuItem()
        {
            
        }

        /// <summary>
        /// Creates a new <see cref="UserMenuItem"/> object from given <see cref="MenuItemDefinition"/>.
        /// 给<see cref="MenuItemDefinition"/>创建一个新的<see cref="UserMenuItem"/>对象
        /// </summary>
        internal UserMenuItem(MenuItemDefinition menuItemDefinition, ILocalizationContext localizationContext)
        {
            Name = menuItemDefinition.Name;
            Icon = menuItemDefinition.Icon;
            DisplayName = menuItemDefinition.DisplayName.Localize(localizationContext);
            Order = menuItemDefinition.Order;
            Url = menuItemDefinition.Url;
            CustomData = menuItemDefinition.CustomData;
            Items = new List<UserMenuItem>();
        }
    }
}