using System.Data.Common;
using System.Data.Entity;
using Abp.Zero.EntityFramework;
using Derrick.Authorization.Roles;
using Derrick.Authorization.Users;
using Derrick.Chat;
using Derrick.Friendships;
using Derrick.MultiTenancy;
using Derrick.Storage;

namespace Derrick.EntityFramework
{
    /// <summary>
    /// ABP Zero DB上下文
    /// </summary>
    public class AbpZeroTemplateDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        /* Define an IDbSet for each entity of the application */
        /// <summary>
        /// 二进制对象
        /// </summary>
        public virtual IDbSet<BinaryObject> BinaryObjects { get; set; }
        /// <summary>
        /// 好友
        /// </summary>
        public virtual IDbSet<Friendship> Friendships { get; set; }
        /// <summary>
        /// 聊天消息
        /// </summary>
        public virtual IDbSet<ChatMessage> ChatMessages { get; set; }

        /* Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         * But it may cause problems when working Migrate.exe of EF. ABP works either way.         * 
         */
        public AbpZeroTemplateDbContext()
            : base("Default")
        {
            
        }

        /* This constructor is used by ABP to pass connection string defined in AbpZeroTemplateDataModule.PreInitialize.
         * Notice that, actually you will not directly create an instance of AbpZeroTemplateDbContext since ABP automatically handles it.
         */
        public AbpZeroTemplateDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        /* This constructor is used in tests to pass a fake/mock connection.
         */
        public AbpZeroTemplateDbContext(DbConnection dbConnection)
            : base(dbConnection, true)
        {

        }
    }
}
