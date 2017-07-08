using Abp.MultiTenancy;
using Abp.Zero.Configuration;

namespace Derrick.Authorization.Roles
{
    /// <summary>
    /// APP 角色配置
    /// </summary>
    public static class AppRoleConfig
    {
        /// <summary>
        /// 角色配置
        /// </summary>
        /// <param name="roleManagementConfig">角色管理配置</param>
        public static void Configure(IRoleManagementConfig roleManagementConfig)
        {
            //Static host roles 
            //静态宿主角色

            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Host.Admin,
                    MultiTenancySides.Host)
                );

            //Static tenant roles
            //静态商户角色

            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Tenants.Admin,
                    MultiTenancySides.Tenant)
                );
            //静态用户角色
            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Tenants.User,
                    MultiTenancySides.Tenant)
                );
        }
    }
}
