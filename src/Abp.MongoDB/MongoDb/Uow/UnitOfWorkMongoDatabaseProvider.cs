using Abp.Dependency;
using Abp.Domain.Uow;
using MongoDB.Driver;

namespace Abp.MongoDb.Uow
{
    /// <summary>
    /// Implements <see cref="IMongoDatabaseProvider"/> that gets database from active unit of work.
    /// <see cref="IMongoDatabaseProvider"/>的实现从有效的工作单元中获取数据库
    /// </summary>
    public class UnitOfWorkMongoDatabaseProvider : IMongoDatabaseProvider, ITransientDependency
    {
        /// <summary>
        /// MongoDB
        /// </summary>
        public MongoDatabase Database { get { return ((MongoDbUnitOfWork)_currentUnitOfWork.Current).Database; } }

        /// <summary>
        /// 当前工作单元提供者
        /// </summary>
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWork;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUnitOfWork"></param>
        public UnitOfWorkMongoDatabaseProvider(ICurrentUnitOfWorkProvider currentUnitOfWork)
        {
            _currentUnitOfWork = currentUnitOfWork;
        }
    }
}