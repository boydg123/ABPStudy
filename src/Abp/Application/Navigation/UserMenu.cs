using System.Collections.Generic;
using Abp.Localization;

namespace Abp.Application.Navigation
{
    /// <summary>
    /// Represents a menu shown to the user.
    /// 表示向用户显示的菜单
    /// </summary>
    public class UserMenu
    {
        /// <summary>
        /// Unique name of the menu in the application. 
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Display name of the menu.
        /// 菜单显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// A custom object related to this menu item.
        /// 菜单关联的自定义数据
        /// </summary>
        public object CustomData { get; set; }

        /// <summary>
        /// Menu items (first level).
        /// 菜单列表(一级菜单)
        /// </summary>
        public IList<UserMenuItem> Items { get; set; }

        /// <summary>
        /// Creates a new <see cref="UserMenu"/> object.
        /// 创建一个新的 <see cref="UserMenu"/> 对象
        /// </summary>
        public UserMenu()
        {
            
        }

        /// <summary>
        /// Creates a new <see cref="UserMenu"/> object from given <see cref="MenuDefinition"/>.
        /// 给<see cref="MenuDefinition"/>创建一个新的<see cref="UserMenu"/>对象
        /// </summary>
        internal UserMenu(MenuDefinition menuDefinition, ILocalizationContext localizationContext)
        {
            Name = menuDefinition.Name;
            DisplayName = menuDefinition.DisplayName.Localize(localizationContext);
            CustomData = menuDefinition.CustomData;
            Items = new List<UserMenuItem>();
        }
    }
}