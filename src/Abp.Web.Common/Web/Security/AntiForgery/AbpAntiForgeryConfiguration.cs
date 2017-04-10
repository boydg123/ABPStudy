namespace Abp.Web.Security.AntiForgery
{
    /// <summary>
    /// ABP防伪配置默认实现
    /// </summary>
    public class AbpAntiForgeryConfiguration : IAbpAntiForgeryConfiguration
    {
        /// <summary>
        /// 令牌Cookie名称
        /// </summary>
        public string TokenCookieName { get; set; }

        /// <summary>
        /// 令牌Header名称
        /// </summary>
        public string TokenHeaderName { get; set; }

        /// <summary>
        /// 构造函数：初始化两个默认名称
        /// </summary>
        public AbpAntiForgeryConfiguration()
        {
            TokenCookieName = "XSRF-TOKEN";
            TokenHeaderName = "X-XSRF-TOKEN";
        }
    }
}