using Abp.Dependency;

namespace Abp.Domain.Policies
{
    /// <summary>
    /// This interface can be implemented by all Policy classes/interfaces to identify them by convention.
    /// 此接口可以由所有策略类/接口实现，以按约定标识它们
    /// </summary>
    public interface IPolicy : ITransientDependency
    {

    }
}
