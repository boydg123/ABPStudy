using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.MongoDb.Configuration;
using MongoDB.Driver;

namespace Abp.MongoDb.Uow
{
    /// <summary>
    /// Implements Unit of work for MongoDB.
    /// MongoDB工作单元实现
    /// </summary>
    public class MongoDbUnitOfWork : UnitOfWorkBase, ITransientDependency
    {
        /// <summary>
        /// 获取一哥MongoDB数据库引用
        /// </summary>
        public MongoDatabase Database { get; private set; }

        /// <summary>
        /// ABP MongoDB模块配置
        /// </summary>
        private readonly IAbpMongoDbModuleConfiguration _configuration;

        /// <summary>
        /// 构造函数.
        /// </summary>
        public MongoDbUnitOfWork(
            IAbpMongoDbModuleConfiguration configuration, 
            IConnectionStringResolver connectionStringResolver,
            IUnitOfWorkFilterExecuter filterExecuter,
            IUnitOfWorkDefaultOptions defaultOptions)
            : base(
                  connectionStringResolver, 
                  defaultOptions,
                  filterExecuter)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 开启工作单元
        /// </summary>
        #pragma warning disable
        protected override void BeginUow()
        {
            //TODO: MongoClientExtensions.GetServer(MongoClient)' is obsolete: 'Use the new API instead.
            Database = new MongoClient(_configuration.ConnectionString)
                .GetServer()
                .GetDatabase(_configuration.DatatabaseName);
        }
        #pragma warning restore

        /// <summary>
        /// 保存所有的更改
        /// </summary>
        public override void SaveChanges()
        {

        }

        /// <summary>
        /// 异步保存所有更改
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

        /// <summary>
        /// 释放工作单元
        /// </summary>
        #pragma warning restore 1998
        protected override void DisposeUow()
        {

        }
    }
}