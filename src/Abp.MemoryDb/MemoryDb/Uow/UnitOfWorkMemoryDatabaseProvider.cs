using Abp.Dependency;
using Abp.Domain.Uow;

namespace Abp.MemoryDb.Uow
{
    /// <summary>
    /// Implements <see cref="IMemoryDatabaseProvider"/> that gets database from active unit of work.
    /// <see cref="IMemoryDatabaseProvider"/>的实现从有效的工作单元中获取内存数据库
    /// </summary>
    public class UnitOfWorkMemoryDatabaseProvider : IMemoryDatabaseProvider, ITransientDependency
    {
        /// <summary>
        /// 内存数据库
        /// </summary>
        public MemoryDatabase Database { get { return ((MemoryDbUnitOfWork)_currentUnitOfWork.Current).Database; } }

        /// <summary>
        /// 当前工作单元提供者
        /// </summary>
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWork;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUnitOfWork"></param>
        public UnitOfWorkMemoryDatabaseProvider(ICurrentUnitOfWorkProvider currentUnitOfWork)
        {
            _currentUnitOfWork = currentUnitOfWork;
        }
    }
}