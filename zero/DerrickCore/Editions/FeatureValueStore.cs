using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.Runtime.Caching;
using Derrick.Authorization.Users;
using Derrick.MultiTenancy;

namespace Derrick.Editions
{
    /// <summary>
    /// 功能值存储
    /// </summary>
    public class FeatureValueStore : AbpFeatureValueStore<Tenant, User>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheManager">缓存管理</param>
        /// <param name="tenantFeatureSettingRepository">商户功能设置仓储</param>
        /// <param name="tenantRepository">商户仓储</param>
        /// <param name="editionFeatureSettingRepository">版本功能设置仓储</param>
        /// <param name="featureManager">功能管理</param>
        /// <param name="unitOfWorkManager">工作单元管理</param>
        public FeatureValueStore(
            ICacheManager cacheManager,
            IRepository<TenantFeatureSetting, long> tenantFeatureSettingRepository,
            IRepository<Tenant> tenantRepository,
            IRepository<EditionFeatureSetting, long> editionFeatureSettingRepository,
            IFeatureManager featureManager,
            IUnitOfWorkManager unitOfWorkManager)
            : base(cacheManager,
                  tenantFeatureSettingRepository,
                  tenantRepository,
                  editionFeatureSettingRepository,
                  featureManager,
                  unitOfWorkManager)
        {
        }
    }
}
