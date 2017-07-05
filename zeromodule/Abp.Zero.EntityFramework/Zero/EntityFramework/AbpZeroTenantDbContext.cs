using System.Data.Common;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;

namespace Abp.Zero.EntityFramework
{
    /// <summary>
    /// ABP Zero商户数据库上下文
    /// </summary>
    /// <typeparam name="TRole">角色类型</typeparam>
    /// <typeparam name="TUser">用户类型</typeparam>
    [MultiTenancySide(MultiTenancySides.Host)]
    public abstract class AbpZeroTenantDbContext<TRole, TUser> : AbpZeroCommonDbContext<TRole, TUser>
        where TRole : AbpRole<TUser>
        where TUser : AbpUser<TUser>
    {
        /// <summary>
        /// Default constructor.Do not directly instantiate this class. Instead, use dependency injection!
        /// 默认构造函数。不要直接实例化这个类，相反，使用依赖注入
        /// </summary>
        protected AbpZeroTenantDbContext()
        {

        }

        /// <summary>
        /// Constructor with connection string parameter.
        /// 连接字符串参数的构造函数
        /// </summary>
        /// <param name="nameOrConnectionString">Connection string or a name in connection strings in configuration file / 连接字符串或在配置文件中的连接字符串Name</param>
        protected AbpZeroTenantDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        /// <summary>
        /// This constructor can be used for unit tests.
        /// 这个构造函数可被用于单元测试
        /// </summary>
        protected AbpZeroTenantDbContext(DbConnection dbConnection, bool contextOwnsConnection)
            : base(dbConnection, contextOwnsConnection)
        {

        }
    }
}