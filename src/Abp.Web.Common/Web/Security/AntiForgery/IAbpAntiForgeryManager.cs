namespace Abp.Web.Security.AntiForgery
{
    /// <summary>
    /// ABP防伪管理器
    /// </summary>
    public interface IAbpAntiForgeryManager
    {
        /// <summary>
        /// MVC和API间通用的防伪配置
        /// </summary>
        IAbpAntiForgeryConfiguration Configuration { get; }

        /// <summary>
        /// 生成令牌
        /// </summary>
        /// <returns>令牌字符串</returns>
        string GenerateToken();
    }
}
