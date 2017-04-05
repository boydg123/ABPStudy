using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Features;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Runtime.Session;

namespace Abp.Application.Navigation
{
    /// <summary>
    /// 用户导航管理类
    /// </summary>
    public class UserNavigationManager : IUserNavigationManager, ITransientDependency
    {
        /// <summary>
        /// 用户权限检查器
        /// </summary>
        public IPermissionChecker PermissionChecker { get; set; }

        /// <summary>
        /// ABP Session接口
        /// </summary>
        public IAbpSession AbpSession { get; set; }

        /// <summary>
        /// 导航管理器
        /// </summary>
        private readonly INavigationManager _navigationManager;
        /// <summary>
        /// 本地化上下文
        /// </summary>
        private readonly ILocalizationContext _localizationContext;
        /// <summary>
        /// 解决依赖关系的IOC接口
        /// </summary>
        private readonly IIocResolver _iocResolver;

        /// <summary>
        /// 构造函数。创建新的 <see cref="UserNavigationManager"/>
        /// </summary>
        /// <param name="navigationManager">导航管理器</param>
        /// <param name="localizationContext">本地化上下文</param>
        /// <param name="iocResolver">解决依赖关系的IOC接口</param>
        public UserNavigationManager(
            INavigationManager navigationManager,
            ILocalizationContext localizationContext,
            IIocResolver iocResolver)
        {
            _navigationManager = navigationManager;
            _localizationContext = localizationContext;
            _iocResolver = iocResolver;
            PermissionChecker = NullPermissionChecker.Instance;
            AbpSession = NullAbpSession.Instance;
        }

        /// <summary>
        /// 获取指定用户和菜单名称的用户菜单
        /// </summary>
        /// <param name="menuName">菜单名称</param>
        /// <param name="user">用户标识。Null表示匿名用户</param>
        /// <returns></returns>
        public async Task<UserMenu> GetMenuAsync(string menuName, UserIdentifier user)
        {
            var menuDefinition = _navigationManager.Menus.GetOrDefault(menuName);
            if (menuDefinition == null)
            {
                throw new AbpException("There is no menu with given name: " + menuName);
            }

            var userMenu = new UserMenu(menuDefinition, _localizationContext);
            await FillUserMenuItems(user, menuDefinition.Items, userMenu.Items);
            return userMenu;
        }
        /// <summary>
        /// 获取指定用户的菜单集合
        /// </summary>
        /// <param name="user">用户标识</param>
        /// <returns></returns>
        public async Task<IReadOnlyList<UserMenu>> GetMenusAsync(UserIdentifier user)
        {
            var userMenus = new List<UserMenu>();

            foreach (var menu in _navigationManager.Menus.Values)
            {
                userMenus.Add(await GetMenuAsync(menu.Name, user));
            }

            return userMenus;
        }
        /// <summary>
        /// 填充用户菜单项
        /// </summary>
        /// <param name="user">用户表示</param>
        /// <param name="menuItemDefinitions">菜单项集合</param>
        /// <param name="userMenuItems">用户菜单项</param>
        /// <returns></returns>
        private async Task<int> FillUserMenuItems(UserIdentifier user, IList<MenuItemDefinition> menuItemDefinitions, IList<UserMenuItem> userMenuItems)
        {
            //TODO: Can be optimized by re-using FeatureDependencyContext.

            var addedMenuItemCount = 0;

            using (var featureDependencyContext = _iocResolver.ResolveAsDisposable<FeatureDependencyContext>())
            {
                featureDependencyContext.Object.TenantId = user == null ? null : user.TenantId;

                foreach (var menuItemDefinition in menuItemDefinitions)
                {
                    if (menuItemDefinition.RequiresAuthentication && user == null)
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(menuItemDefinition.RequiredPermissionName) && (user == null || !(await PermissionChecker.IsGrantedAsync(user, menuItemDefinition.RequiredPermissionName))))
                    {
                        continue;
                    }

                    if (menuItemDefinition.FeatureDependency != null &&
                        (AbpSession.MultiTenancySide == MultiTenancySides.Tenant || (user != null && user.TenantId != null)) &&
                        !(await menuItemDefinition.FeatureDependency.IsSatisfiedAsync(featureDependencyContext.Object)))
                    {
                        continue;
                    }

                    var userMenuItem = new UserMenuItem(menuItemDefinition, _localizationContext);
                    if (menuItemDefinition.IsLeaf || (await FillUserMenuItems(user, menuItemDefinition.Items, userMenuItem.Items)) > 0)
                    {
                        userMenuItems.Add(userMenuItem);
                        ++addedMenuItemCount;
                    }
                }
            }

            return addedMenuItemCount;
        }
    }
}
