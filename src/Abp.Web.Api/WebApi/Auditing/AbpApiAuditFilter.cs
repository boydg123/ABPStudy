using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Abp.Auditing;
using Abp.Dependency;
using Abp.WebApi.Validation;

namespace Abp.WebApi.Auditing
{
    /// <summary>
    /// ABP Api 审计过滤器
    /// </summary>
    public class AbpApiAuditFilter : IActionFilter, ITransientDependency
    {
        /// <summary>
        /// 获取或设置一个布尔值，该值指示能否为一个程序元素指定多个指示属性实例。
        /// </summary>
        public bool AllowMultiple => false;

        /// <summary>
        /// 审计帮助类
        /// </summary>
        private readonly IAuditingHelper _auditingHelper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="auditingHelper">审计帮助类</param>
        public AbpApiAuditFilter(IAuditingHelper auditingHelper)
        {
            _auditingHelper = auditingHelper;
        }

        /// <summary>
        /// 异步执行Action Filter
        /// </summary>
        /// <param name="actionContext">HttpAction上下文</param>
        /// <param name="cancellationToken"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            var method = actionContext.ActionDescriptor.GetMethodInfoOrNull();
            if (method == null || !ShouldSaveAudit(actionContext))
            {
                return await continuation();
            }

            var auditInfo = _auditingHelper.CreateAuditInfo(
                method,
                actionContext.ActionArguments
            );

            var stopwatch = Stopwatch.StartNew();

            try
            {
                return await continuation();
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
                await _auditingHelper.SaveAsync(auditInfo);
            }
        }

        /// <summary>
        /// 是否保存审计
        /// </summary>
        /// <param name="context">HttpAction上下文</param>
        /// <returns></returns>
        private bool ShouldSaveAudit(HttpActionContext context)
        {
            if (context.ActionDescriptor.IsDynamicAbpAction())
            {
                return false;
            }

            return _auditingHelper.ShouldSaveAudit(context.ActionDescriptor.GetMethodInfoOrNull(), true);
        }
    }
}