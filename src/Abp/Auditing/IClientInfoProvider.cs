namespace Abp.Auditing
{
    /// <summary>
    /// 客户端信息提供者
    /// </summary>
    public interface IClientInfoProvider
    {
        /// <summary>
        /// 浏览器信息
        /// </summary>
        string BrowserInfo { get; }

        /// <summary>
        /// 客户端IP地址
        /// </summary>
        string ClientIpAddress { get; }

        /// <summary>
        /// 电脑名称
        /// </summary>
        string ComputerName { get; }
    }
}