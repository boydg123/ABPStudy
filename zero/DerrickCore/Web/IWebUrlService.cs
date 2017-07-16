namespace Derrick.Web
{
    /// <summary>
    /// Web Url服务
    /// </summary>
    public interface IWebUrlService
    {
        /// <summary>
        /// 获取站点根地址
        /// </summary>
        /// <param name="tenancyName">商户名称</param>
        /// <returns></returns>
        string GetSiteRootAddress(string tenancyName = null);
    }
}
