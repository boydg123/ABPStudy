using System;
using System.Collections.Generic;

namespace Abp.Collections.Extensions
{
    /// <summary>
    /// Extension methods for Collections.
    /// Collections类型的护展方法
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Checks whatever given collection object is null or has no item.
        /// 检查给定的对象是否为null，或者为空
        /// </summary>
        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source == null || source.Count <= 0;
        }

        /// <summary>
        /// Adds an item to the collection if it's not already in the collection.
        /// 添加一个项到集合如果它不存在于集合中
        /// </summary>
        /// <param name="source">Collection / 源集合</param>
        /// <param name="item">Item to check and add / 需检测并添加的项</param>
        /// <typeparam name="T">Type of the items in the collection / 集合中项的类型</typeparam>
        /// <returns>Returns True if added, returns False if not. / 如果添加了返回True,没添加返回false</returns>
        public static bool AddIfNotContains<T>(this ICollection<T> source, T item)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (source.Contains(item))
            {
                return false;
            }

            source.Add(item);
            return true;
        }
    }
}