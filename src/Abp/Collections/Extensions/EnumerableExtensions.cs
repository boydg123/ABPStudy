using System;
using System.Collections.Generic;
using System.Linq;

namespace Abp.Collections.Extensions
{
    /// <summary> 
    /// Extension methods for <see cref="IEnumerable{T}"/>.
    /// <see cref="IEnumerable{T}"/>护展方法.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Concatenates the members of a constructed <see cref="IEnumerable{T}"/> collection of type System.String, using the specified separator between each member.
        /// This is a shortcut for string.Join(...)
        /// 以字符串的形式，用给定的分隔符连接<see cref="IEnumerable{T}"/>的成员
        /// 这是string.Join(...)的快捷使用方式
        /// </summary>
        /// <param name="source">A collection that contains the strings to concatenate. / 包含要连接的字符串集合</param>
        /// <param name="separator">The string to use as a separator. separator is included in the returned string only if values has more than one element. / 分隔符，如果集合中包含一个以上的成员，分隔符将会被包含在返回的字符串中</param>
        /// <returns>A string that consists of the members of values delimited by the separator string. If values has no members, the method returns System.String.Empty. / 包含集合中所有成员，他们使用分隔符连接，如果集合中没有成员，返回String.Empty.</returns>
        public static string JoinAsString(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source);
        }

        /// <summary>
        /// Concatenates the members of a collection, using the specified separator between each member.
        /// This is a shortcut for string.Join(...)
        /// 以字符串的形式，用给定的分隔符连接<see cref="IEnumerable{T}"/>的成员
        /// 这是string.Join(...)的快捷使用方式
        /// </summary>
        /// <param name="source">A collection that contains the objects to concatenate. / 包含要连接的字符串集合</param>
        /// <param name="separator">The string to use as a separator. separator is included in the returned string only if values has more than one element. / 分隔符，如果集合中包含一个以上的成员，分隔符将会被包含在返回的字符串中</param>
        /// <typeparam name="T">The type of the members of values. / 值成员类型</typeparam>
        /// <returns>A string that consists of the members of values delimited by the separator string. If values has no members, the method returns System.String.Empty. / 包含集合中所有成员，他们使用分隔符连接，如果集合中没有成员，返回String.Empty.</returns>
        public static string JoinAsString<T>(this IEnumerable<T> source, string separator)
        {
            return string.Join(separator, source);
        }

        /// <summary>
        /// Filters a <see cref="IEnumerable{T}"/> by given predicate if given condition is true.
        /// 如果给定的条件为true，就使用该条件过滤<see cref="IEnumerable{T}"/> 
        /// </summary>
        /// <param name="source">Enumerable to apply filtering / 应用过滤条件的可枚举集合</param>
        /// <param name="condition">A boolean value / 一个布尔值</param>
        /// <param name="predicate">Predicate to filter the enumerable / 过滤集合的表达式</param>
        /// <returns>Filtered or not filtered enumerable based on <paramref name="condition"/> / 过滤后的集合<see cref="condition"/></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
        {
            return condition
                ? source.Where(predicate)
                : source;
        }

        /// <summary>
        /// Filters a <see cref="IEnumerable{T}"/> by given predicate if given condition is true.
        /// 如果给定的条件为true，就使用该条件过滤<see cref="IEnumerable{T}"/> 
        /// </summary>
        /// <param name="source">Enumerable to apply filtering / 应用过滤条件的可枚举集合</param>
        /// <param name="condition">A boolean value / 一个布尔值</param>
        /// <param name="predicate">Predicate to filter the enumerable / 过滤集合的表达式</param>
        /// <returns>Filtered or not filtered enumerable based on <paramref name="condition"/> / 过滤后的集合<see cref="condition"/></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, int, bool> predicate)
        {
            return condition
                ? source.Where(predicate)
                : source;
        }
    }
}
