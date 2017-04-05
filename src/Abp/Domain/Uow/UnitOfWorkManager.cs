using System.Transactions;
using Abp.Dependency;

namespace Abp.Domain.Uow
{
    /// <summary>
    /// Unit of work manager.
    /// 工作单元管理器 将修饰符internal 改为 public(Derrick 2017/04/02)
    /// </summary>
    public class UnitOfWorkManager : IUnitOfWorkManager, ITransientDependency
    {
        /// <summary>
        /// IOC解析器
        /// </summary>
        private readonly IIocResolver _iocResolver;
        /// <summary>
        /// 当前工作单元提供者
        /// </summary>
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;
        /// <summary>
        /// 工作单元默认选项
        /// </summary>
        private readonly IUnitOfWorkDefaultOptions _defaultOptions;

        /// <summary>
        /// 获取当前工作单元
        /// </summary>
        public IActiveUnitOfWork Current
        {
            get { return _currentUnitOfWorkProvider.Current; }
        }

        /// <summary>
        /// 创建一个新的<see cref="UnitOfWorkManager"/>对象
        /// </summary>
        /// <param name="iocResolver">IOC解析器</param>
        /// <param name="currentUnitOfWorkProvider">当前工作单元提供者</param>
        /// <param name="defaultOptions">工作单元默认选项</param>
        public UnitOfWorkManager(
            IIocResolver iocResolver,
            ICurrentUnitOfWorkProvider currentUnitOfWorkProvider,
            IUnitOfWorkDefaultOptions defaultOptions)
        {
            _iocResolver = iocResolver;
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
            _defaultOptions = defaultOptions;
        }

        /// <summary>
        /// 启动一个新的工作单元
        /// </summary>
        /// <returns>一个能结束工作单元的处理器</returns>
        public IUnitOfWorkCompleteHandle Begin()
        {
            return Begin(new UnitOfWorkOptions());
        }

        /// <summary>
        /// 启动一个新的工作单元
        /// </summary>
        /// <param name="scope">事物范围</param>
        /// <returns>一个能结束工作单元的处理器</returns>
        public IUnitOfWorkCompleteHandle Begin(TransactionScopeOption scope)
        {
            return Begin(new UnitOfWorkOptions { Scope = scope });
        }

        /// <summary>
        /// 启动一个新的工作单元
        /// </summary>
        /// <param name="options">工作单元选项</param>
        /// <returns>一个能结束工作单元的处理器</returns>
        public IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options)
        {
            options.FillDefaultsForNonProvidedOptions(_defaultOptions);

            if (options.Scope == TransactionScopeOption.Required && _currentUnitOfWorkProvider.Current != null)
            {
                return new InnerUnitOfWorkCompleteHandle();
            }

            var uow = _iocResolver.Resolve<IUnitOfWork>();

            uow.Completed += (sender, args) =>
            {
                _currentUnitOfWorkProvider.Current = null;
            };

            uow.Failed += (sender, args) =>
            {
                _currentUnitOfWorkProvider.Current = null;
            };

            uow.Disposed += (sender, args) =>
            {
                _iocResolver.Release(uow);
            };

            uow.Begin(options);

            _currentUnitOfWorkProvider.Current = uow;

            return uow;
        }
    }
}