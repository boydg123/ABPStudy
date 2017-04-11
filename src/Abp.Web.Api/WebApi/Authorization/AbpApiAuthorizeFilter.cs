using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Events.Bus;
using Abp.Events.Bus.Exceptions;
using Abp.Localization;
using Abp.Logging;
using Abp.Web;
using Abp.Web.Models;
using Abp.WebApi.Configuration;
using Abp.WebApi.Validation;

namespace Abp.WebApi.Authorization
{
    /// <summary>
    /// ABP Api授权过滤器
    /// </summary>
    public class AbpApiAuthorizeFilter : IAuthorizationFilter, ITransientDependency
    {
        /// <summary>
        /// 获取或设置一个布尔值，该值指示能否为一个程序元素指定多个指示属性实例。
        /// </summary>
        public bool AllowMultiple => false;

        /// <summary>
        /// 授权帮助类
        /// </summary>
        private readonly IAuthorizationHelper _authorizationHelper;

        /// <summary>
        /// ABP Web Api配置
        /// </summary>
        private readonly IAbpWebApiConfiguration _configuration;

        /// <summary>
        /// 本地化管理器
        /// </summary>
        private readonly ILocalizationManager _localizationManager;

        /// <summary>
        /// 事件总线
        /// </summary>
        private readonly IEventBus _eventBus;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="authorizationHelper">授权帮助类</param>
        /// <param name="configuration">ABP Web Api配置</param>
        /// <param name="localizationManager">本地化管理器</param>
        /// <param name="eventBus">事件总线</param>
        public AbpApiAuthorizeFilter(
            IAuthorizationHelper authorizationHelper, 
            IAbpWebApiConfiguration configuration,
            ILocalizationManager localizationManager,
            IEventBus eventBus)
        {
            _authorizationHelper = authorizationHelper;
            _configuration = configuration;
            _localizationManager = localizationManager;
            _eventBus = eventBus;
        }

        /// <summary>
        /// 执行授权过滤器 - 异步
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        public virtual async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(
            HttpActionContext actionContext,
            CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {
            if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any() ||
                actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                return await continuation();
            }
            
            var methodInfo = actionContext.ActionDescriptor.GetMethodInfoOrNull();
            if (methodInfo == null)
            {
                return await continuation();
            }

            if (actionContext.ActionDescriptor.IsDynamicAbpAction())
            {
                return await continuation();
            }

            try
            {
                await _authorizationHelper.AuthorizeAsync(methodInfo);
                return await continuation();
            }
            catch (AbpAuthorizationException ex)
            {
                LogHelper.Logger.Warn(ex.ToString(), ex);
                _eventBus.Trigger(this, new AbpHandledExceptionData(ex));
                return CreateUnAuthorizedResponse(actionContext);
            }
        }

        /// <summary>
        /// 创建没有授权的响应
        /// </summary>
        /// <param name="actionContext">HttpAction上下文</param>
        /// <returns></returns>
        protected virtual HttpResponseMessage CreateUnAuthorizedResponse(HttpActionContext actionContext)
        {
            HttpStatusCode statusCode;
            ErrorInfo error;

            if (actionContext.RequestContext.Principal?.Identity?.IsAuthenticated ?? false)
            {
                statusCode = HttpStatusCode.Forbidden;
                error = new ErrorInfo(
                    _localizationManager.GetString(AbpWebConsts.LocalizaionSourceName, "DefaultError403"),
                    _localizationManager.GetString(AbpWebConsts.LocalizaionSourceName, "DefaultErrorDetail403")
                );
            }
            else
            {
                statusCode = HttpStatusCode.Unauthorized;
                error = new ErrorInfo(
                    _localizationManager.GetString(AbpWebConsts.LocalizaionSourceName, "DefaultError401"),
                    _localizationManager.GetString(AbpWebConsts.LocalizaionSourceName, "DefaultErrorDetail401")
                );
            }

            var response = new HttpResponseMessage(statusCode)
            {
                Content = new ObjectContent<AjaxResponse>(
                    new AjaxResponse(error, true),
                    _configuration.HttpConfiguration.Formatters.JsonFormatter
                )
            };

            return response;
        }
    }
}