using Abp.Dependency;

namespace Abp.Domain.Repositories
{
    /// <summary>
    /// This interface must be implemented by all repositories to identify them by convention.
    /// 通过约定去标识所有实现此接口的仓储
    /// Implement generic version instead of this one.
    /// 实现泛型版本来取代它
    /// </summary>
    public interface IRepository : ITransientDependency
    {
        
    }
}