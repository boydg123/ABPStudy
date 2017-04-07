using Abp.Modules;

namespace Abp.Owin
{
    /// <summary>
    /// OWIN integration module for ABP.
    /// ABP 的OWIN集成模块
    /// </summary>
    [DependsOn(typeof (AbpKernelModule))]
    public class AbpOwinModule : AbpModule
    {
        //nothing to do...
    }
}
