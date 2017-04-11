using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Abp.Extensions;
using Abp.WebApi.Extensions;

namespace Abp.Web.Security.AntiForgery
{
    /// <summary>
    /// ABP Web Api 防伪管理器扩展
    /// </summary>
    public static class AbpAntiForgeryManagerWebApiExtensions
    {
        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="manager">ABP防伪管理器</param>
        /// <param name="headers">Http响应头</param>
        public static void SetCookie(this IAbpAntiForgeryManager manager, HttpResponseHeaders headers)
        {
            headers.SetCookie(new Cookie(manager.Configuration.TokenCookieName, manager.GenerateToken()));
        }

        /// <summary>
        /// 是否有效
        /// </summary>
        /// <param name="manager">ABP防伪管理器</param>
        /// <param name="headers">Http响应头</param>
        /// <returns></returns>
        public static bool IsValid(this IAbpAntiForgeryManager manager, HttpRequestHeaders headers)
        {
            var cookieTokenValue = GetCookieValue(manager, headers);
            if (cookieTokenValue.IsNullOrEmpty())
            {
                return true;
            }

            var headerTokenValue = GetHeaderValue(manager, headers);
            if (headerTokenValue.IsNullOrEmpty())
            {
                return false;
            }

            return manager.As<IAbpAntiForgeryValidator>().IsValid(cookieTokenValue, headerTokenValue);
        }

        /// <summary>
        /// 获取Cookie值
        /// </summary>
        /// <param name="manager">ABP防伪管理器</param>
        /// <param name="headers">Http响应头</param>
        /// <returns></returns>
        private static string GetCookieValue(IAbpAntiForgeryManager manager, HttpRequestHeaders headers)
        {
            var cookie = headers.GetCookies(manager.Configuration.TokenCookieName).LastOrDefault();
            if (cookie == null)
            {
                return null;
            }

            return cookie[manager.Configuration.TokenCookieName].Value;
        }

        /// <summary>
        /// 获取Http响应头值
        /// </summary>
        /// <param name="manager">ABP防伪管理器</param>
        /// <param name="headers">Http响应头</param>
        /// <returns></returns>
        private static string GetHeaderValue(IAbpAntiForgeryManager manager, HttpRequestHeaders headers)
        {
            IEnumerable<string> headerValues;
            if (!headers.TryGetValues(manager.Configuration.TokenHeaderName, out headerValues))
            {
                return null;
            }

            var headersArray = headerValues.ToArray();
            if (!headersArray.Any())
            {
                return null;
            }
            
            return headersArray.Last().Split(", ").Last();
        }
    }
}
