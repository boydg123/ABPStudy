namespace Abp.Web.Security.AntiForgery
{
    /// <summary>
    /// Common configuration shared between ASP.NET Core, ASP.NET MVC and ASP.NET Web API.
    /// 在MVC和API之间共享的通用配置(ABP防伪配置)
    /// </summary>
    public interface IAbpAntiForgeryConfiguration
    {
        /// <summary>
        /// Get/sets cookie name to transfer Anti Forgery token between server and client.Default value: "XSRF-TOKEN".
        /// 获取/设置cookie名称用来在服务端与客户端之间传输防伪令牌。默认值："XSRF-TOKEN"
        /// </summary>
        string TokenCookieName { get; set; }

        /// <summary>
        /// Get/sets header name to transfer Anti Forgery token from client to the server.Default value: "X-XSRF-TOKEN". 
        /// 获取/设置header名称用来在服务端与客户端之间传输防伪令牌。默认值："X-XSRF-TOKEN"
        /// </summary>
        string TokenHeaderName { get; set; }
    }
}