using Abp.MultiTenancy;
using Derrick.Authorization.Users;

namespace Derrick.MultiTenancy
{
    /// <summary>
    /// Represents a Tenant in the system.A tenant is a isolated customer for the application which has it's own users, roles and other application entities.
    /// 表示系统中的一个商户，商户是系统中的独立客户，它有自己的用户，角色以及其他应用程序实体。
    /// </summary>
    public class Tenant : AbpTenant<User>
    {
        //Can add application specific tenant properties here
        //可以在此添加具体的商户属性

        /// <summary>
        /// 构造函数
        /// </summary>
        protected Tenant()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tenancyName">商户名</param>
        /// <param name="name">名称</param>
        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {

        }
    }
}