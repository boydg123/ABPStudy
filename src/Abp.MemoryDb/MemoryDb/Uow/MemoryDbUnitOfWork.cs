using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.MemoryDb.Configuration;

namespace Abp.MemoryDb.Uow
{
    /// <summary>
    /// Implements Unit of work for MemoryDb.
    /// 内存数据库工作单元实现
    /// </summary>
    public class MemoryDbUnitOfWork : UnitOfWorkBase, ITransientDependency
    {
        /// <summary>
        /// Gets a reference to Memory Database.
        /// 获取一个内存数据库引用
        /// </summary>
        public MemoryDatabase Database { get; private set; }

        /// <summary>
        /// 内存数据库模块配置引用
        /// </summary>
        private readonly IAbpMemoryDbModuleConfiguration _configuration;

        /// <summary>
        /// 内存数据库
        /// </summary>
        private readonly MemoryDatabase _memoryDatabase;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MemoryDbUnitOfWork(
            IAbpMemoryDbModuleConfiguration configuration, 
            MemoryDatabase memoryDatabase, 
            IConnectionStringResolver connectionStringResolver,
            IUnitOfWorkFilterExecuter filterExecuter,
            IUnitOfWorkDefaultOptions defaultOptions)
            : base(
                  connectionStringResolver, 
                  defaultOptions,
                  filterExecuter)
        {
            _configuration = configuration;
            _memoryDatabase = memoryDatabase;
        }

        /// <summary>
        /// 开启工作单元
        /// </summary>
        protected override void BeginUow()
        {
            Database = _memoryDatabase;
        }

        /// <summary>
        /// 保存所有的更改
        /// </summary>
        public override void SaveChanges()
        {

        }

        /// <summary>
        /// 异步保存所有的更改
        /// </summary>
        /// <returns></returns>
        #pragma warning disable 1998
        public override async Task SaveChangesAsync()
        {

        }
        #pragma warning restore 1998

        /// <summary>
        /// 完成工作单元
        /// </summary>
        protected override void CompleteUow()
        {

        }

        /// <summary>
        /// 异步完成工作单元
        /// </summary>
        /// <returns></returns>
        #pragma warning disable 1998
        protected override async Task CompleteUowAsync()
        {

        }
        #pragma warning restore 1998

        /// <summary>
        /// 释放工作单元
        /// </summary>
        protected override void DisposeUow()
        {

        }
    }
}