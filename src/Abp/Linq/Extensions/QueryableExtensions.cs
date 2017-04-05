using System;
using System.Linq;
using System.Linq.Expressions;
using Abp.Application.Services.Dto;

namespace Abp.Linq.Extensions
{
    /// <summary>
    /// Some useful extension methods for <see cref="IQueryable{T}"/>.
    /// <see cref="IQueryable{T}"/>一些有用的扩展方法
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Used for paging. Can be used as an alternative to Skip(...).Take(...) chaining.
        /// 用于翻页，使用Skip(...).Take(...)调用链来实现
        /// </summary>
        public static IQueryable<T> PageBy<T>(this IQueryable<T> query, int skipCount, int maxResultCount)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            return query.Skip(skipCount).Take(maxResultCount);
        }

        /// <summary>
        /// Used for paging with an <see cref="IPagedResultRequest"/> object.
        /// 翻页，使用<see cref="IPagedResultRequest"/>对象作为参数
        /// </summary>
        /// <param name="query">Queryable to apply paging / 用于翻页的可查询对象</param>
        /// <param name="pagedResultRequest">An object implements <see cref="IPagedResultRequest"/> interface / 一个实现了接口<see cref="IPagedResultRequest"/> 的对象</param>
        public static IQueryable<T> PageBy<T>(this IQueryable<T> query, IPagedResultRequest pagedResultRequest)
        {
            return query.PageBy(pagedResultRequest.SkipCount, pagedResultRequest.MaxResultCount);
        }

        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// 如果给定的条件为真，使用给定的表达式过滤<see cref="IQueryable{T}"/>
        /// </summary>
        /// <param name="query">Queryable to apply filtering / 用于过滤的可查询对象</param>
        /// <param name="condition">A boolean value / 一个布尔值</param>
        /// <param name="predicate">Predicate to filter the query / 过滤查询的表达式</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition"/> / 值，过虑或不过滤query后的结果</returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }

        /// <summary>
        /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
        /// 如果给定的条件为真，使用给定的表达式过滤<see cref="IQueryable{T}"/>
        /// </summary>
        /// <param name="query">Queryable to apply filtering / 用于过滤的可查询对象</param>
        /// <param name="condition">A boolean value / 一个布尔值</param>
        /// <param name="predicate">Predicate to filter the query / 过滤查询的表达式</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition"/> / 值，过虑或不过滤query后的结果</returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, int, bool>> predicate)
        {
            return condition
                ? query.Where(predicate)
                : query;
        }
    }
}
