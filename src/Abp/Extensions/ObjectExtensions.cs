using System;
using System.Globalization;
using System.Linq;

namespace Abp.Extensions
{
    /// <summary>
    /// Extension methods for all objects.
    /// object的扩展方法
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Used to simplify and beautify casting an object to a type. 
        /// 使object转换为一个特定类型时简洁、优雅
        /// </summary>
        /// <typeparam name="T">Type to be casted / 目标类型</typeparam>
        /// <param name="obj">Object to cast / 待转对象</param>
        /// <returns>Casted object / 转换后的对象</returns>
        public static T As<T>(this object obj)
            where T : class
        {
            return (T)obj;
        }

        /// <summary>
        /// Converts given object to a value type using <see cref="Convert.ChangeType(object,System.TypeCode)"/> method.
        /// 使用 <see cref="Convert.ChangeType(object,System.TypeCode)"/>方法，转换一个对象到指定的类型。
        /// </summary>
        /// <param name="obj">Object to be converted / 待转对象</param>
        /// <typeparam name="T">Type of the target object / 目标类型</typeparam>
        /// <returns>Converted object / 转换后的对象</returns>
        public static T To<T>(this object obj)
            where T : struct
        {
            return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Check if an item is in a list.
        /// 检查一个项是否存在于列表中
        /// </summary>
        /// <param name="item">Item to check / 待检查项</param>
        /// <param name="list">List of items / 列表</param>
        /// <typeparam name="T">Type of the items / 列表项的类型</typeparam>
        public static bool IsIn<T>(this T item, params T[] list)
        {
            return list.Contains(item);
        }
    }
}
