using System;
using System.Threading.Tasks;

namespace Abp.Domain.Uow
{
    /// <summary>
    /// Used to complete a unit of work.This interface can not be injected or directly used.Use <see cref="IUnitOfWorkManager"/> instead.
    /// 用于完成一个工作单元,此接口不能被注入或直接使用。使用 <see cref="IUnitOfWorkManager"/> 代替.
    /// </summary>
    public interface IUnitOfWorkCompleteHandle : IDisposable
    {
        /// <summary>
        /// Completes this unit of work.It saves all changes and commit transaction if exists.
        /// 完成此工作单元,保存所有的更变，如果有事务，并提交事务
        /// </summary>
        void Complete();

        /// <summary>
        /// Completes this unit of work.It saves all changes and commit transaction if exists.
        /// 完成此工作单元-异步。保存所有的更变，如果有事务，并提交事务
        /// </summary>
        Task CompleteAsync();
    }
}