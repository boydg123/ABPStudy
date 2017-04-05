using System.Threading.Tasks;

namespace Abp.Authorization
{
    /// <summary>
    /// Defines interface to check a dependency.
    /// 定义检查依赖项的接口
    /// </summary>
    public interface IPermissionDependency
    {
        /// <summary>
        /// Checks if permission dependency is satisfied.
        /// 检查是否满足权限依赖
        /// </summary>
        /// <param name="context">Context. / 权限依赖上下文</param>
        Task<bool> IsSatisfiedAsync(IPermissionDependencyContext context);
    }
}