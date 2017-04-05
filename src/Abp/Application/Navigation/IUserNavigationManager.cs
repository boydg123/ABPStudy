using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.Application.Navigation
{
    /// <summary>
    /// 基于用户管理的导航管理接口.
    /// </summary>
    public interface IUserNavigationManager
    {
        /// <summary>
        /// 根据固定用户标识，菜单名称获取用户菜单.
        /// </summary>
        /// <param name="menuName">菜单名称</param>
        /// <param name="user">用户标识, 如果是Null则表示匿名用户</param>
        Task<UserMenu> GetMenuAsync(string menuName, UserIdentifier user);

        /// <summary>
        /// 根据用户标识，获取当前用户所有菜单.
        /// </summary>
        /// <param name="user">用户标识，如果是Null则表示匿名用户</param>
        Task<IReadOnlyList<UserMenu>> GetMenusAsync(UserIdentifier user);
    }
}