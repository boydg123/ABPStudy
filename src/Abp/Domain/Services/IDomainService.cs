using Abp.Dependency;

namespace Abp.Domain.Services
{
    /// <summary>
    /// This interface must be implemented by all domain services to identify them by convention.
    /// 所有的领域服务类必须此接口，通过约定标识它们
    /// </summary>
    public interface IDomainService : ITransientDependency
    {

    }
}