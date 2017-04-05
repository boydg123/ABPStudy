using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Abp.Aspects;
using Abp.Threading;
using Castle.DynamicProxy;

namespace Abp.Auditing
{
    /// <summary>
    /// 审计拦截器
    /// </summary>
    internal class AuditingInterceptor : IInterceptor
    {
        /// <summary>
        /// 审计帮助类
        /// </summary>
        private readonly IAuditingHelper _auditingHelper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="auditingHelper"></param>
        public AuditingInterceptor(IAuditingHelper auditingHelper)
        {
            _auditingHelper = auditingHelper;
        }

        /// <summary>
        /// 拦截
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            if (AbpCrossCuttingConcerns.IsApplied(invocation.InvocationTarget, AbpCrossCuttingConcerns.Auditing))
            {
                invocation.Proceed();
                return;
            }

            if (!_auditingHelper.ShouldSaveAudit(invocation.MethodInvocationTarget))
            {
                invocation.Proceed();
                return;
            }

            var auditInfo = _auditingHelper.CreateAuditInfo(invocation.MethodInvocationTarget, invocation.Arguments);

            if (AsyncHelper.IsAsyncMethod(invocation.Method))
            {
                PerformAsyncAuditing(invocation, auditInfo);
            }
            else
            {
                PerformSyncAuditing(invocation, auditInfo);
            }
        }

        /// <summary>
        /// 进行同步审计
        /// </summary>
        /// <param name="invocation">代理方法</param>
        /// <param name="auditInfo">审计信息</param>
        private void PerformSyncAuditing(IInvocation invocation, AuditInfo auditInfo)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                auditInfo.Exception = ex;
                throw;
            }
            finally
            {
                stopwatch.Stop();
                auditInfo.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
                _auditingHelper.Save(auditInfo);
            }
        }

        /// <summary>
        /// 进行同步审计
        /// </summary>
        /// <param name="invocation">代理方法</param>
        /// <param name="auditInfo">审计信息</param>
        private void PerformAsyncAuditing(IInvocation invocation, AuditInfo auditInfo)
        {
            var stopwatch = Stopwatch.StartNew();

            invocation.Proceed();

            if (invocation.Method.ReturnType == typeof(Task))
            {
                invocation.ReturnValue = InternalAsyncHelper.AwaitTaskWithFinally(
                    (Task) invocation.ReturnValue,
                    exception => SaveAuditInfo(auditInfo, stopwatch, exception)
                    );
            }
            else //Task<TResult>
            {
                invocation.ReturnValue = InternalAsyncHelper.CallAwaitTaskWithFinallyAndGetResult(
                    invocation.Method.ReturnType.GenericTypeArguments[0],
                    invocation.ReturnValue,
                    exception => SaveAuditInfo(auditInfo, stopwatch, exception)
                    );
            }
        }

        /// <summary>
        /// 保存审计
        /// </summary>
        /// <param name="auditInfo">审计信息</param>
        /// <param name="stopwatch"></param>
        /// <param name="exception">异常</param>
        private void SaveAuditInfo(AuditInfo auditInfo, Stopwatch stopwatch, Exception exception)
        {
            stopwatch.Stop();
            auditInfo.Exception = exception;
            auditInfo.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);

            _auditingHelper.Save(auditInfo);
        }
    }
}