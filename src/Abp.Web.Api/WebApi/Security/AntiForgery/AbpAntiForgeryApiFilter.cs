using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Abp.Dependency;
using Abp.Web.Security.AntiForgery;
using Abp.WebApi.Configuration;
using Abp.WebApi.Controllers.Dynamic.Selectors;
using Abp.WebApi.Validation;
using Castle.Core.Logging;

namespace Abp.WebApi.Security.AntiForgery
{
    /// <summary>
    /// ABP Api防伪过滤器
    /// </summary>
    public class AbpAntiForgeryApiFilter : IAuthorizationFilter, ITransientDependency
    {
        /// <summary>
        /// 日志引用
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// 是否允许多选
        /// </summary>
        public bool AllowMultiple => false;

        private readonly IAbpAntiForgeryManager _abpAntiForgeryManager;
        private readonly IAbpWebApiConfiguration _webApiConfiguration;
        private readonly IAbpAntiForgeryWebConfiguration _antiForgeryWebConfiguration;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="abpAntiForgeryManager"></param>
        /// <param name="webApiConfiguration"></param>
        /// <param name="antiForgeryWebConfiguration"></param>
        public AbpAntiForgeryApiFilter(
            IAbpAntiForgeryManager abpAntiForgeryManager, 
            IAbpWebApiConfiguration webApiConfiguration,
            IAbpAntiForgeryWebConfiguration antiForgeryWebConfiguration)
        {
            _abpAntiForgeryManager = abpAntiForgeryManager;
            _webApiConfiguration = webApiConfiguration;
            _antiForgeryWebConfiguration = antiForgeryWebConfiguration;
            Logger = NullLogger.Instance;
        }

        /// <summary>
        /// 执行验证过滤 - 异步
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(
            HttpActionContext actionContext,
            CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {
            var methodInfo = actionContext.ActionDescriptor.GetMethodInfoOrNull();
            if (methodInfo == null)
            {
                return await continuation();
            }

            if (!_abpAntiForgeryManager.ShouldValidate(_antiForgeryWebConfiguration, methodInfo, actionContext.Request.Method.ToHttpVerb(), _webApiConfiguration.IsAutomaticAntiForgeryValidationEnabled))
            {
                return await continuation();
            }

            if (!_abpAntiForgeryManager.IsValid(actionContext.Request.Headers))
            {
                return CreateErrorResponse(actionContext, "Empty or invalid anti forgery header token.");
            }

            return await continuation();
        }

        /// <summary>
        /// 创建错误响应信息
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        protected virtual HttpResponseMessage CreateErrorResponse(HttpActionContext actionContext, string reason)
        {
            Logger.Warn(reason);
            Logger.Warn("Requested URI: " + actionContext.Request.RequestUri);
            return new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = reason };
        }
    }
}