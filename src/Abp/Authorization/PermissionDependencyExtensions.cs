using Abp.Threading;

namespace Abp.Authorization
{
    /// <summary>
    /// Extension methods for <see cref="IPermissionDependency"/>.
    /// <see cref="IPermissionDependency"/>的扩展方法
    /// </summary>
    public static class PermissionDependencyExtensions
    {
        /// <summary>
        /// Checks if permission dependency is satisfied.
        /// 检查是否满足权限依赖
        /// </summary>
        /// <param name="permissionDependency">The permission dependency / 权限依赖</param>
        /// <param name="context">Context. / 权限依赖上下文</param>
        public static bool IsSatisfied(this IPermissionDependency permissionDependency, IPermissionDependencyContext context)
        {
            return AsyncHelper.RunSync(() => permissionDependency.IsSatisfiedAsync(context));
        }
    }
}