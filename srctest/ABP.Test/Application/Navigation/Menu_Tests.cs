using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using Abp.Application.Navigation;
using Abp;

namespace Abp.Test.Application.Navigation
{
    /// <summary>
    /// 导航测试
    /// </summary>
    public class Menu_Tests : TestBaseWithLocalManager
    {
        /// <summary>
        /// 测试导航系统
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Test_Menu_System()
        {
            var testCase = new NavigationTestCase();

            //检查顶级菜单数量
            var mainMenuDefination = testCase.NavigationManager.MainMenu;
            mainMenuDefination.Items.Count.ShouldBe(1);

            //检查管理员下2级导航
            var adminMeunDefination = mainMenuDefination.GetItemByNameOrNull("Abp.Zero.Administration");
            adminMeunDefination.ShouldNotBe(null);
            adminMeunDefination.Items.Count.ShouldBe(3);

            //检查用户菜单
            var userMenu = await testCase.UserNavigationManager.GetMenuAsync(mainMenuDefination.Name, new UserIdentifier(1, 1));
            userMenu.Items.Count.ShouldBe(1);
            //用户管理菜单
            var userAdminMenu = userMenu.Items.FirstOrDefault(m => m.Name == "Abp.Zero.Administration");
            userAdminMenu.ShouldNotBe(null);

            userAdminMenu.Items.FirstOrDefault(i => i.Name == "Abp.Zero.Administration.User").ShouldNotBe(null); //加了权限限制，但是授权了
            userAdminMenu.Items.FirstOrDefault(i => i.Name == "Abp.Zero.Administration.Role").ShouldBe(null); //加了权限限制，没有授权
            userAdminMenu.Items.FirstOrDefault(i => i.Name == "Abp.Zero.Administration.Setting").ShouldNotBe(null); // 没有加权限限制
        }
    }
}
