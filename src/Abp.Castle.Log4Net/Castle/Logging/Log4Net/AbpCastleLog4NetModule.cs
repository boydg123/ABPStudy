using Abp.Modules;

namespace Abp.Castle.Logging.Log4Net
{
    /// <summary>
    /// ABP Castle Log4Net module.
    /// ABP Castle Log4Net 模块,该模块依赖于<see cref="AbpKernelModule"/>
    /// </summary>
    [DependsOn(typeof(AbpKernelModule))]
    public class AbpCastleLog4NetModule : AbpModule
    {

    }
}