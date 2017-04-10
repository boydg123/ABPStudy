using System.Text;
using System.Threading.Tasks;
using Abp.Application.Navigation;
using Abp.Dependency;
using Abp.Json;
using Abp.Runtime.Session;

namespace Abp.Web.Navigation
{
    /// <summary>
    /// 导航脚本管理器默认实现
    /// </summary>
    internal class NavigationScriptManager : INavigationScriptManager, ITransientDependency
    {
        /// <summary>
        /// ABP Session
        /// </summary>
        public IAbpSession AbpSession { get; set; }

        /// <summary>
        /// 用户导航管理器
        /// </summary>
        private readonly IUserNavigationManager _userNavigationManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userNavigationManager">用户导航管理器</param>
        public NavigationScriptManager(IUserNavigationManager userNavigationManager)
        {
            _userNavigationManager = userNavigationManager;
            AbpSession = NullAbpSession.Instance;
        }

        /// <summary>
        /// 获取导航脚本
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetScriptAsync()
        {
            var userMenus = await _userNavigationManager.GetMenusAsync(AbpSession.ToUserIdentifier());

            var sb = new StringBuilder();
            sb.AppendLine("(function() {");

            sb.AppendLine("    abp.nav = {};");
            sb.AppendLine("    abp.nav.menus = {");

            for (int i = 0; i < userMenus.Count; i++)
            {
                AppendMenu(sb, userMenus[i]);
                if (userMenus.Count - 1 > i)
                {
                    sb.Append(" , ");
                }
            }

            sb.AppendLine("    };");

            sb.AppendLine("})();");

            return sb.ToString();
        }

        /// <summary>
        /// 追加菜单
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="menu">用户菜单</param>
        private static void AppendMenu(StringBuilder sb, UserMenu menu)
        {
            sb.AppendLine("        '" + menu.Name + "': {");

            sb.AppendLine("            name: '" + menu.Name + "',");

            if (menu.DisplayName != null)
            {
                sb.AppendLine("            displayName: '" + menu.DisplayName + "',");
            }

            if (menu.CustomData != null)
            {
                sb.AppendLine("            customData: " + menu.CustomData.ToJsonString(true) + ",");
            }

            sb.Append("            items: ");

            if (menu.Items.Count <= 0)
            {
                sb.AppendLine("[]");
            }
            else
            {
                sb.Append("[");
                for (int i = 0; i < menu.Items.Count; i++)
                {
                    AppendMenuItem(16, sb, menu.Items[i]);
                    if (menu.Items.Count - 1 > i)
                    {
                        sb.Append(" , ");
                    }
                }
                sb.AppendLine("]");
            }

            sb.AppendLine("            }");
        }

        /// <summary>
        /// 追加菜单项
        /// </summary>
        /// <param name="indentLength"></param>
        /// <param name="sb"></param>
        /// <param name="menuItem">菜单项</param>
        private static void AppendMenuItem(int indentLength, StringBuilder sb, UserMenuItem menuItem)
        {
            sb.AppendLine("{");

            sb.AppendLine(new string(' ', indentLength + 4) + "name: '" + menuItem.Name + "',");
            sb.AppendLine(new string(' ', indentLength + 4) + "order: '" + menuItem.Order + "',");

            if (!string.IsNullOrEmpty(menuItem.Icon))
            {
                sb.AppendLine(new string(' ', indentLength + 4) + "icon: '" + menuItem.Icon.Replace("'", @"\'") + "',");
            }

            if (!string.IsNullOrEmpty(menuItem.Url))
            {
                sb.AppendLine(new string(' ', indentLength + 4) + "url: '" + menuItem.Url.Replace("'", @"\'") + "',");
            }

            if (menuItem.DisplayName != null)
            {
                sb.AppendLine(new string(' ', indentLength + 4) + "displayName: '" + menuItem.DisplayName.Replace("'", @"\'") + "',");
            }

            if (menuItem.CustomData != null)
            {
                sb.AppendLine(new string(' ', indentLength + 4) + "customData: " + menuItem.CustomData.ToJsonString(true) + ",");
            }

            sb.Append(new string(' ', indentLength + 4) + "items: [");

            for (int i = 0; i < menuItem.Items.Count; i++)
            {
                AppendMenuItem(24, sb, menuItem.Items[i]);
                if (menuItem.Items.Count - 1 > i)
                {
                    sb.Append(" , ");
                }
            }

            sb.AppendLine("]");

            sb.Append(new string(' ', indentLength) + "}");
        }
    }
}