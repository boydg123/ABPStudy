using System;
using System.Data.Entity;
using Abp.MultiTenancy;

namespace Abp.EntityFramework
{
    /// <summary>
    /// 数据库上下文提供者接口
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IDbContextProvider<out TDbContext>
        where TDbContext : DbContext
    {
        /// <summary>
        /// 获取数据库上下文
        /// </summary>
        /// <returns>数据库上下文对象</returns>
        TDbContext GetDbContext();

        /// <summary>
        /// 获取数据库上下文
        /// </summary>
        /// <param name="multiTenancySide">表示多租户双方中的一方</param>
        /// <returns>数据库上下文</returns>
        TDbContext GetDbContext(MultiTenancySides? multiTenancySide );
    }
}