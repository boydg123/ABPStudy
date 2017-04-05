using System;
using System.Collections.Generic;
using Abp.Application.Features;
using Abp.Collections.Extensions;
using Abp.Localization;

namespace Abp.Application.Navigation
{
    /// <summary>
    /// 代表 <see cref="MenuDefinition"/>的一个菜单项.
    /// </summary>
    public class MenuItemDefinition : IHasMenuItemDefinitions
    {
        /// <summary>
        /// 当前应用程序唯一的菜单项名称. 
        /// 可用于查找当前菜单项.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 菜单项的显示名称[必填].
        /// </summary>
        public ILocalizableString DisplayName { get; set; }
        
        /// <summary>
        /// 在菜单中显示的排序号[选填] .
        /// </summary>
        public int Order { get; set; }
        
        /// <summary>
        /// 菜单图标(如果存在)[选填]
        /// </summary>
        public string Icon { get; set; }
        
        /// <summary>
        /// 当前被选中导航的Url[选填].
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 权限名称，有权限的用户才能看到此菜单项.
        /// 可选.
        /// </summary>
        public string RequiredPermissionName { get; set; }

        /// <summary>
        /// 功能依赖.
        /// 可选.
        /// </summary>
        public IFeatureDependency FeatureDependency { get; set; }

        /// <summary>
        /// 仅仅当授权用户能看到该菜单项的时候设置为True.
        /// </summary>
        public bool RequiresAuthentication { get; set; }

        /// <summary>
        /// 如果该菜单项没有子项<see cref="Items"/>，则设置为True.
        /// </summary>
        public bool IsLeaf
        {
            get { return Items.IsNullOrEmpty(); }
        }

        /// <summary>
        /// 存储当前菜单项关联的自定义数据[选填].
        /// </summary>
        public object CustomData { get; set; }

        /// <summary>
        /// 该菜单项的子项[选填].
        /// </summary>
        public virtual IList<MenuItemDefinition> Items { get; private set; }

        /// <summary>
        /// 创建一个新的 <see cref="MenuItemDefinition"/> 对象.
        /// </summary>
        public MenuItemDefinition(
            string name, 
            ILocalizableString displayName, 
            string icon = null, 
            string url = null, 
            bool requiresAuthentication = false, 
            string requiredPermissionName = null, 
            int order = 0, 
            object customData = null,
            IFeatureDependency featureDependency = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            if (displayName == null)
            {
                throw new ArgumentNullException("displayName");
            }

            Name = name;
            DisplayName = displayName;
            Icon = icon;
            Url = url;
            RequiresAuthentication = requiresAuthentication;
            RequiredPermissionName = requiredPermissionName;
            Order = order;
            CustomData = customData;
            FeatureDependency = featureDependency;

            Items = new List<MenuItemDefinition>();
        }

        /// <summary>
        /// 添加一个 <see cref="MenuItemDefinition"/> 到 <see cref="Items"/>.
        /// </summary>
        /// <param name="menuItem"><see cref="MenuItemDefinition"/> to be added</param>
        /// <returns>This <see cref="MenuItemDefinition"/> object</returns>
        public MenuItemDefinition AddItem(MenuItemDefinition menuItem)
        {
            Items.Add(menuItem);
            return this;
        }
    }
}
