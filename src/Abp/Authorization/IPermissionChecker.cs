using System.Threading.Tasks;

namespace Abp.Authorization
{
    /// <summary>
    /// This class is used to permissions for users.
    /// 此类用于用户的权限检查
    /// </summary>
    public interface IPermissionChecker
    {
        /// <summary>
        /// Checks if current user is granted for a permission.
        /// 异步检查当前用户是否被授予给定的权限
        /// </summary>
        /// <param name="permissionName">Name of the permission / 权限名称</param>
        Task<bool> IsGrantedAsync(string permissionName);

        /// <summary>
        /// Checks if a user is granted for a permission.
        /// 检查一个用户是否被授予给定的权限
        /// </summary>
        /// <param name="user">User to check / 需要检查的用户编号</param>
        /// <param name="permissionName">Name of the permission / 权限名称</param>
        Task<bool> IsGrantedAsync(UserIdentifier user, string permissionName);
    }
}