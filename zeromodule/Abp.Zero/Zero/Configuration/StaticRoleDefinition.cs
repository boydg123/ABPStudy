using Abp.MultiTenancy;

namespace Abp.Zero.Configuration
{
    /// <summary>
    /// 静态角色定义
    /// </summary>
    public class StaticRoleDefinition
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; private set; }
        /// <summary>
        /// 多租户类型
        /// </summary>
        public MultiTenancySides Side { get; private set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <param name="side">多租户类型</param>
        public StaticRoleDefinition(string roleName, MultiTenancySides side)
        {
            RoleName = roleName;
            Side = side;
        }
    }
}