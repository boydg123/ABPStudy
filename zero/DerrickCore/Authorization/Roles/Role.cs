using Abp.Authorization.Roles;
using Derrick.Authorization.Users;

namespace Derrick.Authorization.Roles
{
    /// <summary>
    /// 表示系统里的一个角色
    /// </summary>
    public class Role : AbpRole<User>
    {
        //Can add application specific role properties here
        //可以在此添加一些特定的角色属性

        /// <summary>
        /// 构造函数
        /// </summary>
        public Role()
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <param name="displayName">显示名</param>
        public Role(int? tenantId, string displayName)
            : base(tenantId, displayName)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <param name="name">名称</param>
        /// <param name="displayName">显示名称</param>
        public Role(int? tenantId, string name, string displayName)
            : base(tenantId, name, displayName)
        {

        }
    }
}
