using System.Collections.Generic;

namespace Abp.Zero.Configuration
{
    /// <summary>
    /// 角色管理配置
    /// </summary>
    public interface IRoleManagementConfig
    {
        List<StaticRoleDefinition> StaticRoles { get; }
    }
}