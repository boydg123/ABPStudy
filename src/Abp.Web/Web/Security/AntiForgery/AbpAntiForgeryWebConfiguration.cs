using System.Collections.Generic;

namespace Abp.Web.Security.AntiForgery
{
    /// <summary>
    /// ABP防伪Web配置
    /// </summary>
    public class AbpAntiForgeryWebConfiguration : IAbpAntiForgeryWebConfiguration
    {
        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 忽略的Http请求
        /// </summary>
        public HashSet<HttpVerb> IgnoredHttpVerbs { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AbpAntiForgeryWebConfiguration()
        {
            IsEnabled = true;
            IgnoredHttpVerbs = new HashSet<HttpVerb> { HttpVerb.Get, HttpVerb.Head, HttpVerb.Options, HttpVerb.Trace };
        }
    }
}