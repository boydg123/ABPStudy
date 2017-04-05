using System.Data.Entity;
using Abp.Domain.Uow;
using Abp.MultiTenancy;

namespace Abp.EntityFramework.Uow
{
    /// <summary>
    /// Implements <see cref="IDbContextProvider{TDbContext}"/> that gets DbContext from active unit of work.
    /// <see cref="IDbContextProvider{TDbContext}"/>的实现，从工作单元中获取数据库上下文
    /// </summary>
    /// <typeparam name="TDbContext">Type of the DbContext / 数据库上下文类型</typeparam>
    public class UnitOfWorkDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : DbContext
    {
        /// <summary>
        /// 当前工作单元提供者
        /// </summary>
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;

        /// <summary>
        /// Creates a new <see cref="UnitOfWorkDbContextProvider{TDbContext}"/>.
        /// 构造函数
        /// </summary>
        /// <param name="currentUnitOfWorkProvider"></param>
        public UnitOfWorkDbContextProvider(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }

        /// <summary>
        /// 获取数据库上下文
        /// </summary>
        /// <returns>数据库上下文对象</returns>
        public TDbContext GetDbContext()
        {
            return GetDbContext(null);
        }

        /// <summary>
        /// 获取数据库上下文
        /// </summary>
        /// <param name="multiTenancySide">表示多租户双方中的一方</param>
        /// <returns>数据库上下文</returns>
        public TDbContext GetDbContext(MultiTenancySides? multiTenancySide)
        {
            return _currentUnitOfWorkProvider.Current.GetDbContext<TDbContext>(multiTenancySide);
        }
    }
}