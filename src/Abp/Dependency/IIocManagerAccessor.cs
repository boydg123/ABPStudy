namespace Abp.Dependency
{
    /// <summary>
    /// 执行依赖注入任务的访问器(IOC管理器访问器)
    /// </summary>
    public interface IIocManagerAccessor
    {
        /// <summary>
        /// IOC管理器
        /// </summary>
        IIocManager IocManager { get; }
    }
}