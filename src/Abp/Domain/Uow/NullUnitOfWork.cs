using System.Threading.Tasks;

namespace Abp.Domain.Uow
{
    /// <summary>
    /// Null implementation of unit of work.It's used if no component registered for <see cref="IUnitOfWork"/>.This ensures working ABP without a database.
    /// 工作单元的Null对象模式实现.如果没有针对 <see cref="IUnitOfWork"/>的组件（实现类）注册时使用.它确保abp在没有数据库的情况下正常运行 
    /// </summary>
    public sealed class NullUnitOfWork : UnitOfWorkBase
    {
        /// <summary>
        /// 保存工作单元中所有的修改。
        /// 这个方法在需要应用修改时调用。
        /// 注意，如果工作单元是事务性的，不仅保存变更，还会在事务失败时，回滚事务。
        /// 一般不用显式调用SaveChages，因为工作单元会自动保存所有变更
        /// </summary>
        public override void SaveChanges()
        {

        }

        /// <summary>
        /// 保存工作单元中所有的修改。
        /// 这个方法在需要应用修改时调用。
        /// 注意，如果工作单元是事务性的，不仅保存变更，还会在事务失败时，回滚事务。
        /// 一般不用显式调用SaveChages，因为工作单元会自动保存所有变更
        /// </summary>
        /// <returns></returns>
        public override Task SaveChangesAsync()
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// 开始工作单元
        /// </summary>
        protected override void BeginUow()
        {

        }

        /// <summary>
        /// 完成工作单元
        /// </summary>
        protected override void CompleteUow()
        {

        }

        /// <summary>
        /// 完成工作单元--异步
        /// </summary>
        /// <returns></returns>
        protected override Task CompleteUowAsync()
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// 销毁工作单元
        /// </summary>
        protected override void DisposeUow()
        {

        }

        /// <summary>
        /// 创建一个<see cref="NullUnitOfWork"/>对象
        /// </summary>
        /// <param name="connectionStringResolver">连接字符串解析器</param>
        /// <param name="defaultOptions">工作单元默认选项</param>
        /// <param name="filterExecuter">工作单元过滤器执行器</param>
        public NullUnitOfWork(
            IConnectionStringResolver connectionStringResolver,
            IUnitOfWorkDefaultOptions defaultOptions,
            IUnitOfWorkFilterExecuter filterExecuter
            ) : base(
                connectionStringResolver,
                defaultOptions,
                filterExecuter)
        {
        }
    }
}
