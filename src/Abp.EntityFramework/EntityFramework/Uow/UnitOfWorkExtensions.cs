using System;
using System.Data.Entity;
using Abp.Domain.Uow;
using Abp.MultiTenancy;

namespace Abp.EntityFramework.Uow
{
    /// <summary>
    /// Extension methods for UnitOfWork.
    /// 工作单元的扩展方法
    /// </summary>
    public static class UnitOfWorkExtensions
    {
        /// <summary>
        /// Gets a DbContext as a part of active unit of work.This method can be called when current unit of work is an <see cref="EfUnitOfWork"/>.
        /// 获取一个数据库上下文作为工作单元的一部分，如果当前工作单元是<see cref="EfUnitOfWork"/>，则当前方法可以被调用。
        /// </summary>
        /// <typeparam name="TDbContext">Type of the DbContext / 数据库上下文的类型</typeparam>
        /// <param name="unitOfWork">Current (active) unit of work / 当前的(激活的)工作单元</param>
        public static TDbContext GetDbContext<TDbContext>(this IActiveUnitOfWork unitOfWork) 
            where TDbContext : DbContext
        {
            return GetDbContext<TDbContext>(unitOfWork, null);
        }

        /// <summary>
        /// 获取一个数据库上下文作为工作单元的一部分，如果当前工作单元是<see cref="EfUnitOfWork"/>，则当前方法可以被调用。
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文的类型</typeparam>
        /// <param name="unitOfWork">当前的(激活的)工作单元</param>
        /// <param name="multiTenancySide">多租户双方中的一方</param>
        /// <returns>数据库上下文</returns>
        public static TDbContext GetDbContext<TDbContext>(this IActiveUnitOfWork unitOfWork, MultiTenancySides? multiTenancySide)
            where TDbContext : DbContext
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }

            if (!(unitOfWork is EfUnitOfWork))
            {
                throw new ArgumentException("unitOfWork is not type of " + typeof(EfUnitOfWork).FullName, nameof(unitOfWork));
            }

            return (unitOfWork as EfUnitOfWork).GetOrCreateDbContext<TDbContext>(multiTenancySide);
        }
    }
}