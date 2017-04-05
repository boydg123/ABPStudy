using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Abp.EntityFramework.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IQueryable"/> and <see cref="IQueryable{T}"/>.
    /// <see cref="IQueryable"/> 和 <see cref="IQueryable{T}"/>的扩展方法
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Specifies the related objects to include in the query results.
        /// 指定要包含在查询结果中的相关对象。
        /// </summary>
        /// <param name="source">The source <see cref="IQueryable"/> on which to call Include. / 要调用的查询源<see cref="IQueryable"/></param>
        /// <param name="condition">A boolean value to determine to include <paramref name="path"/> or not. / 是否包含<paramref name="path"/></param>
        /// <param name="path">The dot-separated list of related objects to return in the query results. / 在查询结果中返回相关对象的点分隔列表</param>
        public static IQueryable IncludeIf(this IQueryable source, bool condition, string path)
        {
            return condition
                ? source.Include(path)
                : source;
        }

        /// <summary>
        /// Specifies the related objects to include in the query results.
        /// 指定要包含在查询结果中的相关对象
        /// </summary>
        /// <param name="source">The source <see cref="IQueryable{T}"/> on which to call Include./ 要调用的查询源</param>
        /// <param name="condition">A boolean value to determine to include <paramref name="path"/> or not. / 是否包含<paramref name="path"/></param>
        /// <param name="path">The dot-separated list of related objects to return in the query results. / 在查询结果中返回相关对象的点分隔列表</param>
        public static IQueryable<T> IncludeIf<T>(this IQueryable<T> source, bool condition, string path)
        {
            return condition
                ? source.Include(path)
                : source;
        }

        /// <summary>
        /// Specifies the related objects to include in the query results.
        /// 指定要包含在查询结果中的相关对象
        /// </summary>
        /// <param name="source">The source <see cref="IQueryable{T}"/> on which to call Include. / 要调用的查询源</param>
        /// <param name="condition">A boolean value to determine to include <paramref name="path"/> or not. / 是否包含<paramref name="path"/></param>
        /// <param name="path">The type of navigation property being included. / 包含导航属性的类型</param>
        public static IQueryable<T> IncludeIf<T, TProperty>(this IQueryable<T> source, bool condition, Expression<Func<T, TProperty>> path)
        {
            return condition
                ? source.Include(path)
                : source;
        }
    }
}