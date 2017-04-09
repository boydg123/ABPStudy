using System;
using System.Net;
using System.Net.Sockets;
using System.Web;
using Castle.Core.Logging;

namespace Abp.Auditing
{
    /// <summary>
    /// Web端审计信息提供者
    /// </summary>
    public class WebAuditInfoProvider : IClientInfoProvider
    {
        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string BrowserInfo => GetBrowserInfo();

        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public string ClientIpAddress => GetClientIpAddress();

        /// <summary>
        /// 电脑名称
        /// </summary>
        public string ComputerName => GetComputerName();

        /// <summary>
        /// 日志记录器
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Http上下文
        /// </summary>
        private readonly HttpContext _httpContext;

        /// <summary>
        /// Creates a new <see cref="WebAuditInfoProvider"/>.
        /// 构造函数
        /// </summary>
        public WebAuditInfoProvider()
        {
            _httpContext = HttpContext.Current;
            Logger = NullLogger.Instance;
        }

        /// <summary>
        /// 获取浏览器信息
        /// </summary>
        /// <returns></returns>
        protected virtual string GetBrowserInfo()
        {
            var httpContext = HttpContext.Current ?? _httpContext;
            if (httpContext?.Request.Browser == null)
            {
                return null;
            }

            return httpContext.Request.Browser.Browser + " / " +
                   httpContext.Request.Browser.Version + " / " +
                   httpContext.Request.Browser.Platform;
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns></returns>
        protected virtual string GetClientIpAddress()
        {
            var httpContext = HttpContext.Current ?? _httpContext;
            if (httpContext?.Request.ServerVariables == null)
            {
                return null;
            }

            var clientIp = httpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ??
                           httpContext.Request.ServerVariables["REMOTE_ADDR"];

            try
            {
                foreach (var hostAddress in Dns.GetHostAddresses(clientIp))
                {
                    if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return hostAddress.ToString();
                    }
                }

                foreach (var hostAddress in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return hostAddress.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Debug(ex.ToString());
            }

            return clientIp;
        }

        /// <summary>
        /// 获取电脑名称
        /// </summary>
        /// <returns></returns>
        protected virtual string GetComputerName()
        {
            var httpContext = HttpContext.Current ?? _httpContext;
            if (httpContext == null || !httpContext.Request.IsLocal)
            {
                return null;
            }

            try
            {
                var clientIp = httpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ??
                               httpContext.Request.ServerVariables["REMOTE_ADDR"];
                return Dns.GetHostEntry(IPAddress.Parse(clientIp)).HostName;
            }
            catch
            {
                return null;
            }
        }
    }
}
