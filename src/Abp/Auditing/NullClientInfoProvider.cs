namespace Abp.Auditing
{
    /// <summary>
    /// <see cref="IClientInfoProvider"/>的Null对象模式实现
    /// </summary>
    public class NullClientInfoProvider : IClientInfoProvider
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static NullClientInfoProvider Instance { get; } = new NullClientInfoProvider();

        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string BrowserInfo => null;

        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public string ClientIpAddress => null;

        /// <summary>
        /// 电脑名称
        /// </summary>
        public string ComputerName => null;
    }
}