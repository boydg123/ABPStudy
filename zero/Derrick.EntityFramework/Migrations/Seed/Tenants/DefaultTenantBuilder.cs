using System.Linq;
using Derrick.Editions;
using Derrick.EntityFramework;

namespace Derrick.Migrations.Seed.Tenants
{
    /// <summary>
    /// 默认商户生成器
    /// </summary>
    public class DefaultTenantBuilder
    {
        /// <summary>
        /// DB上下文
        /// </summary>
        private readonly AbpZeroTemplateDbContext _context;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context">DB上下文</param>
        public DefaultTenantBuilder(AbpZeroTemplateDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 创建
        /// </summary>
        public void Create()
        {
            CreateDefaultTenant();
        }
        /// <summary>
        /// 创建默认商户
        /// </summary>
        private void CreateDefaultTenant()
        {
            //Default tenant

            var defaultTenant = _context.Tenants.FirstOrDefault(t => t.TenancyName == MultiTenancy.Tenant.DefaultTenantName);
            if (defaultTenant == null)
            {
                defaultTenant = new MultiTenancy.Tenant(MultiTenancy.Tenant.DefaultTenantName, MultiTenancy.Tenant.DefaultTenantName);

                var defaultEdition = _context.Editions.FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
                if (defaultEdition != null)
                {
                    defaultTenant.EditionId = defaultEdition.Id;
                }

                _context.Tenants.Add(defaultTenant);
                _context.SaveChanges();
            }
        }
    }
}
