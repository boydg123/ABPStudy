using Xunit;
using Shouldly;
using NSubstitute;
using Castle.MicroKernel.Registration;
using Abp.Authorization;
using Abp.Localization;
using Abp.Configuration.Startup;
using Abp.Application.Features;

namespace ABP.Test.Authorization
{
    public class PermissionDefiniion_Tests : TestBaseWithLocalManager
    {
        /// <summary>
        /// 权限提供者1
        /// </summary>
        public class MyAuthorizationProvider1 : AuthorizationProvider
        {
            public override void SetPermissions(IPermissionDefinitionContext context)
            {
                //为Administration权限创建一个根权限组
                var administration = context.CreatePermission("Derrick.Administration", new FixedLocalizableString("Administration"));
                //在Administration权限组下创建一个"User Management"权限
                var userManagement = administration.CreateChildPermission("Derrick.Administration.UserManagement", new FixedLocalizableString("User Management"));
                //创建"Change Permissions"权限(用户的change permissions)，在"User Management"的子权限
                userManagement.CreateChildPermission("Derrick.Administration.UserManagement.ChangePermissions", new FixedLocalizableString("Change Permissions"));
            }
        }

        /// <summary>
        /// 权限提供者2
        /// </summary>
        public class MyAuthorizationProvider2 : AuthorizationProvider
        {
            public override void SetPermissions(IPermissionDefinitionContext context)
            {
                //获取已存在的根权限组(Derrick.Administration)
                var administration = context.GetPermissionOrNull("Derrick.Administration");
                administration.ShouldNotBe(null);
                //在(Derrick.Administration)权限组下创建"Role Management"权限
                var roleManagement = administration.CreateChildPermission("Derrick.Administration.RoleManagement", new FixedLocalizableString("Role Management"));
                //创建(Create Role：创建角色)权限，"Role Management"下的子权限
                roleManagement.CreateChildPermission("Derrick.Administration.RoleManagement.CreateRole", new FixedLocalizableString("Create Role"));
            }
        }

        [Fact]
        public void Test_PermissionManager()
        {
            var authorizationConfiguration = new AuthorizationConfiguration();
            //添加两个权限提供者
            authorizationConfiguration.Providers.Add<MyAuthorizationProvider1>();
            authorizationConfiguration.Providers.Add<MyAuthorizationProvider2>();
            //注册对象
            localIocManager.IocContainer.Register(Component.For<IFeatureDependencyContext, FeatureDependencyContext>().UsingFactoryMethod(() => new FeatureDependencyContext(localIocManager, Substitute.For<IFeatureChecker>())));
            //权限检查器
            var permissionManager = new PermissionManager(localIocManager, authorizationConfiguration);
            permissionManager.Initialize();
            //在两个权限提供者里面有5个权限定义。
            permissionManager.GetAllPermissions().Count.ShouldBe(5);
            //选获取用户管理权限，然后检查它的子权限
            var userManagement = permissionManager.GetPermissionOrNull("Derrick.Administration.UserManagement");
            userManagement.ShouldNotBe(null);
            userManagement.Children.Count.ShouldBe(1);
            //获取检查权限，然后判断它的父权限
            var changePermissions = permissionManager.GetPermissionOrNull("Derrick.Administration.UserManagement.ChangePermissions");
            changePermissions.ShouldNotBe(null);
            changePermissions.Parent.ShouldBeSameAs(userManagement);

            permissionManager.GetPermissionOrNull("不存在的权限名称").ShouldBe(null);
        }
    }
}
