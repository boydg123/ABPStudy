using System.Data.Entity.Migrations;
using Abp.Events.Bus;
using Abp.Events.Bus.Entities;
using Abp.MultiTenancy;
using Abp.Zero.EntityFramework;
using EntityFramework.DynamicFilters;
using Derrick.Migrations.Seed.Host;
using Derrick.Migrations.Seed.Tenants;

namespace Derrick.Migrations
{
    /// <summary>
    /// 数据库迁移配置
    /// </summary>
    public sealed class Configuration : DbMigrationsConfiguration<EntityFramework.AbpZeroTemplateDbContext>, IMultiTenantSeed
    {
        /// <summary>
        /// ABP商户基类
        /// </summary>
        public AbpTenantBase Tenant { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "AbpZeroTemplate";
        }
        /// <summary>
        /// 初始化种子数据
        /// </summary>
        /// <param name="context">DB上下文</param>
        protected override void Seed(EntityFramework.AbpZeroTemplateDbContext context)
        {
            context.DisableAllFilters();

            context.EntityChangeEventHelper = NullEntityChangeEventHelper.Instance;
            context.EventBus = NullEventBus.Instance;

            if (Tenant == null)
            {
                //Host seed
                new InitialHostDbBuilder(context).Create();

                //Default tenant seed (in host database).
                new DefaultTenantBuilder(context).Create();
                new TenantRoleAndUserBuilder(context, 1).Create();
            }
            else
            {
                //You can add seed for tenant databases using Tenant property...
            }

            context.SaveChanges();
        }
    }
}
