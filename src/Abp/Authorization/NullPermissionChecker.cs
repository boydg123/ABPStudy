using System.Threading.Tasks;

namespace Abp.Authorization
{
    /// <summary>
    /// Null (and default) implementation of <see cref="IPermissionChecker"/>.
    /// <see cref="IPermissionChecker"/>的null或默认实现
    /// </summary>
    public sealed class NullPermissionChecker : IPermissionChecker
    {
        /// <summary>
        /// Singleton instance.
        /// 单例模式
        /// </summary>
        public static NullPermissionChecker Instance { get; } = new NullPermissionChecker();

        /// <summary>
        /// 异步检查当前用户是否被授予给定的权限
        /// </summary>
        /// <param name="permissionName">权限名称</param>
        /// <returns></returns>
        public Task<bool> IsGrantedAsync(string permissionName)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 检查一个用户是否被授予给定的权限
        /// </summary>
        /// <param name="user">需要检查的用户编号</param>
        /// <param name="permissionName">权限名称</param>
        /// <returns></returns>
        public Task<bool> IsGrantedAsync(UserIdentifier user, string permissionName)
        {
            return Task.FromResult(true);
        }
    }
}