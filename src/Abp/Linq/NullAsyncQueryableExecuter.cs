using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abp.Linq
{
    /// <summary>
    /// NULL对象异步查询执行者
    /// </summary>
    public class NullAsyncQueryableExecuter : IAsyncQueryableExecuter
    {
        /// <summary>
        /// <see cref="NullAsyncQueryableExecuter"/> 实例
        /// </summary>
        public static NullAsyncQueryableExecuter Instance { get; } = new NullAsyncQueryableExecuter();

        /// <summary>
        /// 获取数量 - 异步
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="queryable">类型的查询数据集</param>
        /// <returns>查询的数量</returns>
        public Task<int> CountAsync<T>(IQueryable<T> queryable)
        {
            return Task.FromResult(queryable.Count());
        }

        /// <summary>
        /// 转换成List - 异步
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="queryable">类型的查询数据集</param>
        /// <returns></returns>
        public Task<List<T>> ToListAsync<T>(IQueryable<T> queryable)
        {
            return Task.FromResult(queryable.ToList());
        }

        /// <summary>
        /// 获取默认或第一个
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="queryable">类型的查询数据集</param>
        /// <returns></returns>
        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> queryable)
        {
            return Task.FromResult(queryable.FirstOrDefault());
        }
    }
}