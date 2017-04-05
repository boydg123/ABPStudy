using System;
using System.Collections.Generic;
using Abp.Localization;

namespace Abp.Application.Navigation
{
    /// <summary>
    /// 代表一个应用程序的导航菜单.
    /// </summary>
    public class MenuDefinition : IHasMenuItemDefinitions
    {
        /// <summary>
        /// 菜单名称[必填].
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 菜单显示名称[必填].
        /// </summary>
        public ILocalizableString DisplayName { get; set; }

        /// <summary>
        /// 存储当前菜单相关的数据[可选].
        /// </summary>
        public object CustomData { get; set; }

        /// <summary>
        /// 菜单项 (第一级).
        /// </summary>
        public IList<MenuItemDefinition> Items { get; set; }

        /// <summary>
        /// 创建一个新的 <see cref="MenuDefinition"/> 对象.
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="customData">存储当前菜单相关的数据.</param>
        public MenuDefinition(string name, ILocalizableString displayName, object customData = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "Menu name can not be empty or null.");
            }

            if (displayName == null)
            {
                throw new ArgumentNullException("displayName", "Display name of the menu can not be null.");
            }

            Name = name;
            DisplayName = displayName;
            CustomData = customData;

            Items = new List<MenuItemDefinition>();
        }

        /// <summary>
        /// 添加一个 <see cref="MenuItemDefinition"/> 到 <see cref="Items"/>.
        /// </summary>
        /// <param name="menuItem">被添加的<see cref="MenuItemDefinition"/>对象</param>
        /// <returns>当前 <see cref="MenuDefinition"/> 对象</returns>
        public MenuDefinition AddItem(MenuItemDefinition menuItem)
        {
            Items.Add(menuItem);
            return this;
        }
    }
}
