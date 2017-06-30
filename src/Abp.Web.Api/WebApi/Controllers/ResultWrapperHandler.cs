using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Web.Models;
using Abp.WebApi.Configuration;

namespace Abp.WebApi.Controllers
{
    /// <summary>
    /// Wraps Web API return values by <see cref="AjaxResponse"/>.
    /// 通过<see cref="AjaxResponse"/>包装Web API返回值
    /// </summary>
    public class ResultWrapperHandler : DelegatingHandler, ITransientDependency
    {
        /// <summary>
        /// ABP Web API配置引用
        /// </summary>
        private readonly IAbpWebApiConfiguration _configuration;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public ResultWrapperHandler(IAbpWebApiConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 异步发送
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken).ContinueWith(
                task =>
                {
                    WrapResultIfNeeded(request, task.Result);
                    return task.Result;
                }, cancellationToken);
        }

        /// <summary>
        /// 如果需要则包装Result
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        protected virtual void WrapResultIfNeeded(HttpRequestMessage request, HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                return;
            }

            if (_configuration.SetNoCacheForAllResponses)
            {
                //Based on http://stackoverflow.com/questions/49547/making-sure-a-web-page-is-not-cached-across-all-browsers
                response.Headers.CacheControl = new CacheControlHeaderValue
                {
                    NoCache = true,
                    NoStore = true,
                    MaxAge = TimeSpan.Zero,
                    MustRevalidate = true
                };
            }

            var wrapAttr = HttpActionDescriptorHelper.GetWrapResultAttributeOrNull(request.GetActionDescriptor())
                           ?? _configuration.DefaultWrapResultAttribute;

            if (!wrapAttr.WrapOnSuccess)
            {
                return;
            }

            if (IsIgnoredUrl(request.RequestUri))
            {
                return;
            }

            object resultObject;
            if (!response.TryGetContentValue(out resultObject) || resultObject == null)
            {
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new ObjectContent<AjaxResponse>(
                    new AjaxResponse(),
                    _configuration.HttpConfiguration.Formatters.JsonFormatter
                    );
                return;
            }

            if (resultObject is AjaxResponseBase)
            {
                return;
            }

            response.Content = new ObjectContent<AjaxResponse>(
                new AjaxResponse(resultObject),
                _configuration.HttpConfiguration.Formatters.JsonFormatter
                );
        }

        /// <summary>
        /// 是否忽略URL
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private bool IsIgnoredUrl(Uri uri)
        {
            if (uri == null || uri.AbsolutePath.IsNullOrEmpty())
            {
                return false;
            }

            return _configuration.ResultWrappingIgnoreUrls.Any(url => uri.AbsolutePath.StartsWith(url));
        }
    }
}