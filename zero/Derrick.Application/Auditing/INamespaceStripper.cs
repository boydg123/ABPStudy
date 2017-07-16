namespace Derrick.Auditing
{
    /// <summary>
    /// 命名空间剥离器
    /// </summary>
    public interface INamespaceStripper
    {
        /// <summary>
        /// 剥离命名空间
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        string StripNameSpace(string serviceName);
    }
}