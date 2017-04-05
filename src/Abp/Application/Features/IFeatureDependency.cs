using System.Threading.Tasks;

namespace Abp.Application.Features
{
    /// <summary>
    /// Defines a feature dependency.
    /// 功能依赖接口
    /// </summary>
    public interface IFeatureDependency
    {
        /// <summary>
        /// Checks depended features and returns true if dependencies are satisfied.
        /// 如果依赖是满足的，检查依赖功能返回True
        /// </summary>
        Task<bool> IsSatisfiedAsync(IFeatureDependencyContext context);
    }
}