using Abp.Dependency;

namespace Abp.Application.Services
{
    /// <summary>
    /// This interface must be implemented by all application services to identify them by convention.
    /// 所有的应用服务必须继承此接口，通过约定来识别他们
    /// </summary>
    public interface IApplicationService : ITransientDependency
    {

    }
}
