using System.Threading.Tasks;
using Abp.Threading;
using Castle.DynamicProxy;

namespace Abp.Domain.Uow
{
    /// <summary>
    /// This interceptor is used to manage database connection and transactions.
    /// 此拦截用用管理数据库连接和事务
    /// </summary>
    internal class UnitOfWorkInterceptor : IInterceptor
    {
        /// <summary>
        /// 工作单元管理器
        /// </summary>
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWorkManager">工作单元管理器</param>
        public UnitOfWorkInterceptor(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        /// <summary>
        /// Intercepts a method.
        /// 拦截一个方法
        /// </summary>
        /// <param name="invocation">Method invocation arguments / 方法调用参数</param>
        public void Intercept(IInvocation invocation)
        {
            var unitOfWorkAttr = UnitOfWorkAttribute.GetUnitOfWorkAttributeOrNull(invocation.MethodInvocationTarget);
            if (unitOfWorkAttr == null || unitOfWorkAttr.IsDisabled)
            {
                //No need to a uow / 不需要工作单元
                invocation.Proceed();
                return;
            }

            //No current uow, run a new one / 
            PerformUow(invocation, unitOfWorkAttr.CreateOptions());
        }

        /// <summary>
        /// 准备工作单元
        /// </summary>
        /// <param name="invocation"></param>
        /// <param name="options">工作单元选项</param>
        private void PerformUow(IInvocation invocation, UnitOfWorkOptions options)
        {
            if (AsyncHelper.IsAsyncMethod(invocation.Method))
            {
                PerformAsyncUow(invocation, options);
            }
            else
            {
                PerformSyncUow(invocation, options);
            }
        }

        /// <summary>
        /// 准备同步工作单元
        /// </summary>
        /// <param name="invocation"></param>
        /// <param name="options">工作单元选项</param>
        private void PerformSyncUow(IInvocation invocation, UnitOfWorkOptions options)
        {
            using (var uow = _unitOfWorkManager.Begin(options))
            {
                invocation.Proceed();
                uow.Complete();
            }
        }

        /// <summary>
        /// 准备异步工作单元
        /// </summary>
        /// <param name="invocation"></param>
        /// <param name="options">工作单元选项</param>
        private void PerformAsyncUow(IInvocation invocation, UnitOfWorkOptions options)
        {
            var uow = _unitOfWorkManager.Begin(options);

            invocation.Proceed();

            if (invocation.Method.ReturnType == typeof(Task))
            {
                invocation.ReturnValue = InternalAsyncHelper.AwaitTaskWithPostActionAndFinally(
                    (Task)invocation.ReturnValue,
                    async () => await uow.CompleteAsync(),
                    exception => uow.Dispose()
                    );
            }
            else //Task<TResult>
            {
                invocation.ReturnValue = InternalAsyncHelper.CallAwaitTaskWithPostActionAndFinallyAndGetResult(
                    invocation.Method.ReturnType.GenericTypeArguments[0],
                    invocation.ReturnValue,
                    async () => await uow.CompleteAsync(),
                    (exception) => uow.Dispose()
                    );
            }
        }
    }
}