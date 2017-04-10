using System.Collections.Generic;

namespace Abp.Web.Security.AntiForgery
{
    /// <summary>
    /// Common configuration shared between ASP.NET MVC and ASP.NET Web API.
    /// 在MVC和API之间共享的通用Web配置
    /// </summary>
    public interface IAbpAntiForgeryWebConfiguration
    {
        /// <summary>
        /// Used to enable/disable Anti Forgery token security mechanism of ABP.Default: true (enabled).
        /// 用于在ABP中启用/禁用防伪令牌安全机制。默认值：true(启用)
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// A list of ignored HTTP verbs for Anti Forgery token validation.Default list: Get, Head, Options, Trace.
        /// 
        /// </summary>
        HashSet<HttpVerb> IgnoredHttpVerbs { get; }
    }
}
