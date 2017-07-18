using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Navigation;
using Abp.Localization;
using Abp.Dependency;
using Abp.Configuration.Startup;
using Castle.MicroKernel.Registration;
using Abp.Application.Features;
using NSubstitute;
using Abp.Authorization;
using Abp;

namespace Abp.Test.Application.Navigation
{
    /// <summary>
    /// 导航测试用例
    /// </summary>
    public class NavigationTestCase
    {
        public NavigationManager NavigationManager { get; private set; }
        public UserNavigationManager UserNavigationManager { get; private set; }

        private readonly IIocManager _iocManager;

        public NavigationTestCase()
            : this(new IocManager())
        {

        }

        public NavigationTestCase(IIocManager iocManager)
        {
            _iocManager = iocManager;
            Initialize();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialize()
        {
            //导航提供者应该被注册
            _iocManager.Register<MyNavigationProvider1>();
            _iocManager.Register<MyNavigationProvider2>();
            //准备导航配置
            var configuration = new NavigationConfiguration();
            configuration.Providers.Add<MyNavigationProvider1>();
            configuration.Providers.Add<MyNavigationProvider2>();
            //初始化导航管理
            NavigationManager = new NavigationManager(_iocManager, configuration);
            NavigationManager.Initialize();

            _iocManager.IocContainer.Register(Component.For<IFeatureDependencyContext, FeatureDependencyContext>()
                .UsingFactoryMethod(() => new FeatureDependencyContext(_iocManager, Substitute.For<IFeatureChecker>()))); //伪造一个IFeatureChecker
            //创建用户导航管理用于测试
            UserNavigationManager = new UserNavigationManager(NavigationManager, Substitute.For<ILocalizationContext>(), _iocManager)
            {
                PermissionChecker = CreateMockPermissionChecker()
            };
        }

        /// <summary>
        /// 创建模拟权限检查者
        /// </summary>
        /// <returns></returns>
        private static IPermissionChecker CreateMockPermissionChecker()
        {
            var permissionChecker = Substitute.For<IPermissionChecker>();//模拟一个IPermissionChecker对象
            permissionChecker.IsGrantedAsync(new UserIdentifier(1, 1), "Abp.Zero.UserManagement").Returns(Task.FromResult(true)); //用户管理有权限
            permissionChecker.IsGrantedAsync(new UserIdentifier(1, 1), "Abp.Zero.RoleManagement").Returns(Task.FromResult(false));//角色管理没有权限
            return permissionChecker;
        }

        #region 基础对象
        /// <summary>
        /// 自定义数据类
        /// </summary>
        public class MyCustomDataClass
        {
            public int Data1 { get; set; }
            public string Data2 { get; set; }
        }

        /// <summary>
        /// 导航提供者 - 1
        /// </summary>
        public class MyNavigationProvider1 : NavigationProvider
        {
            /// <summary>
            /// 设置导航
            /// </summary>
            /// <param name="context">导航提供者上下文</param>
            public override void SetNavigation(INavigationProviderContext context)
            {
                context.Manager.MainMenu.AddItem(new MenuItemDefinition("Abp.Zero.Administration", new FixedLocalizableString("Administration"), "fa fa-asterisk", requiresAuthentication: true)
                    .AddItem(new MenuItemDefinition("Abp.Zero.Administration.User", new FixedLocalizableString("User Management"), "fa fa-users", "#/admin/users", requiredPermissionName: "Abp.Zero.UserManagement", customData: "简单测试数据"))
                    .AddItem(new MenuItemDefinition("Abp.Zero.Administration.Role", new FixedLocalizableString("Role Management"), "fa fa-users", "#/admin/roles", requiredPermissionName: "Abp.Zero.RoleManagement")));
            }
        }

        /// <summary>
        /// 导航提供者 - 2
        /// </summary>
        public class MyNavigationProvider2 : NavigationProvider
        {
            public override void SetNavigation(INavigationProviderContext context)
            {
                var adminMenu = context.Manager.MainMenu.GetItemByName("Abp.Zero.Administration");
                adminMenu.AddItem(new MenuItemDefinition("Abp.Zero.Administration.Setting", new FixedLocalizableString("Setting management"), icon: "fa fa-cog", url: "#/admin/settings", customData: new MyCustomDataClass { Data1 = 42, Data2 = "四十二" }));
            }
        }

        #endregion
    }
}
