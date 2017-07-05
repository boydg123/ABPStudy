using System.Data.Common;
using System.Data.Entity;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.BackgroundJobs;
using Abp.MultiTenancy;

namespace Abp.Zero.EntityFramework
{
    /// <summary>
    /// ABP Zero宿主数据库上下文
    /// </summary>
    /// <typeparam name="TTenant">商户类型</typeparam>
    /// <typeparam name="TRole">角色类型</typeparam>
    /// <typeparam name="TUser">用户类型</typeparam>
    [MultiTenancySide(MultiTenancySides.Host)]
    public abstract class AbpZeroHostDbContext<TTenant, TRole, TUser> : AbpZeroCommonDbContext<TRole, TUser>
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser>
        where TUser : AbpUser<TUser>
    {
        /// <summary>
        /// 商户
        /// </summary>
        public virtual IDbSet<TTenant> Tenants { get; set; }

        /// <summary>
        /// 版本.
        /// </summary>
        public virtual IDbSet<Edition> Editions { get; set; }

        /// <summary>
        /// 功能设置.
        /// </summary>
        public virtual IDbSet<FeatureSetting> FeatureSettings { get; set; }

        /// <summary>
        /// 商户功能设置.
        /// </summary>
        public virtual IDbSet<TenantFeatureSetting> TenantFeatureSettings { get; set; }

        /// <summary>
        /// 版本功能设置.
        /// </summary>
        public virtual IDbSet<EditionFeatureSetting> EditionFeatureSettings { get; set; }

        /// <summary>
        /// 后台作业
        /// </summary>
        public virtual IDbSet<BackgroundJobInfo> BackgroundJobs { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public virtual IDbSet<UserAccount> UserAccounts { get; set; }

        /// <summary>
        /// Default constructor.Do not directly instantiate this class. Instead, use dependency injection!
        /// 默认构造函数。不要直接实例化这个类，相反，使用依赖注入
        /// </summary>
        protected AbpZeroHostDbContext()
        {

        }

        /// <summary>
        /// Constructor with connection string parameter.
        /// 连接字符串参数的构造函数
        /// </summary>
        /// <param name="nameOrConnectionString">Connection string or a name in connection strings in configuration file / 连接字符串或在配置文件中的连接字符串Name</param>
        protected AbpZeroHostDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        /// <summary>
        /// This constructor can be used for unit tests.
        /// 这个构造函数可被用于单元测试
        /// </summary>
        protected AbpZeroHostDbContext(DbConnection dbConnection, bool contextOwnsConnection)
            : base(dbConnection, contextOwnsConnection)
        {

        }
    }
}