using System;
using Abp.Collections.Extensions;

namespace Abp.Application.Navigation
{
    /// <summary>
    /// <see cref="IHasMenuItemDefinitions"/>扩展方法
    /// </summary>
    public static class HasMenuItemDefinitionsExtensions
    {
        /// <summary>
        /// 通过名称获取<see cref="MenuItemDefinition"/> 对象.
        /// 如果不存在则抛出异常.
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="name">菜单项名称</param>
        public static MenuItemDefinition GetItemByName(this IHasMenuItemDefinitions source, string name)
        {
            var item = GetItemByNameOrNull(source, name);
            if (item == null)
            {
                throw new ArgumentException("There is no source item with given name: " + name, "name");
            }

            return item;
        }

        /// <summary>
        /// 通过递归的方式根据名称获取 <see cref="MenuItemDefinition"/>对象 .
        /// 如果没找到就返回Null.
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="name">源对象菜单项名称</param>
        public static MenuItemDefinition GetItemByNameOrNull(this IHasMenuItemDefinitions source, string name)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (source.Items.IsNullOrEmpty())
            {
                return null;
            }

            foreach (var subItem in source.Items)
            {
                if (subItem.Name == name)
                {
                    return subItem;
                }

                var subItemSearchResult = GetItemByNameOrNull(subItem, name);
                if (subItemSearchResult != null)
                {
                    return subItemSearchResult;
                }
            }

            return null;
        }
    }
}