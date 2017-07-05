using System.Collections.Generic;

namespace Abp.Zero.Configuration
{
    /// <summary>
    /// 角色管理配置
    /// </summary>
    internal class RoleManagementConfig : IRoleManagementConfig
    {
        /// <summary>
        /// 静态角色列表
        /// </summary>
        public List<StaticRoleDefinition> StaticRoles { get; private set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public RoleManagementConfig()
        {
            StaticRoles = new List<StaticRoleDefinition>();
        }
    }
}