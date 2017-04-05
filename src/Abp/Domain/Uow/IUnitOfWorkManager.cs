using System.Transactions;

namespace Abp.Domain.Uow
{
    /// <summary>
    /// Unit of work manager.Used to begin and control a unit of work.
    /// 工作单元管理器，用于开启以及控制工作单元
    /// </summary>
    public interface IUnitOfWorkManager
    {
        /// <summary>
        /// Gets currently active unit of work (or null if not exists).
        /// 获取目前处理激活状态的工作单元（如果不存在，返回null)
        /// </summary>
        IActiveUnitOfWork Current { get; }

        /// <summary>
        /// Begins a new unit of work.
        /// 启动一个新的工作单元
        /// </summary>
        /// <returns>A handle to be able to complete the unit of work / 一个能结束工作单元的处理器</returns>
        IUnitOfWorkCompleteHandle Begin();

        /// <summary>
        /// Begins a new unit of work.
        /// 启动一个新的工作单元
        /// </summary>
        /// <returns>A handle to be able to complete the unit of work / 一个能结束工作单元的处理器</returns>
        IUnitOfWorkCompleteHandle Begin(TransactionScopeOption scope);

        /// <summary>
        /// Begins a new unit of work.
        /// 启动一个新的工作单元
        /// </summary>
        /// <returns>A handle to be able to complete the unit of work / 一个能结束工作单元的处理器</returns>
        IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options);
    }
}
