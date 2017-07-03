using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;

namespace Abp.MultiTenancy
{
    /// <summary>
    /// 商户缓存实现
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    public class TenantCache<TTenant, TUser> : ITenantCache, IEventHandler<EntityChangedEventData<TTenant>>
        where TTenant : AbpTenant<TUser>
        where TUser : AbpUser<TUser>
    {
        /// <summary>
        /// 缓存管理引用
        /// </summary>
        private readonly ICacheManager _cacheManager;
        /// <summary>
        /// 商户仓储引用
        /// </summary>
        private readonly IRepository<TTenant> _tenantRepository;
        /// <summary>
        /// 工作单元引用
        /// </summary>
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheManager">缓存管理引用</param>
        /// <param name="tenantRepository">商户仓储引用</param>
        /// <param name="unitOfWorkManager">工作单元引用</param>
        public TenantCache(
            ICacheManager cacheManager,
            IRepository<TTenant> tenantRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _cacheManager = cacheManager;
            _tenantRepository = tenantRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }
        /// <summary>
        /// 获取商户缓存项
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <returns></returns>
        public virtual TenantCacheItem Get(int tenantId)
        {
            var cacheItem = GetOrNull(tenantId);

            if (cacheItem == null)
            {
                throw new AbpException("There is no tenant with given id: " + tenantId);
            }

            return cacheItem;
        }
        /// <summary>
        /// 获取商户缓存项
        /// </summary>
        /// <param name="tenancyName">商户名称</param>
        /// <returns></returns>
        public virtual TenantCacheItem Get(string tenancyName)
        {
            var cacheItem = GetOrNull(tenancyName);

            if (cacheItem == null)
            {
                throw new AbpException("There is no tenant with given tenancy name: " + tenancyName);
            }

            return cacheItem;
        }
        /// <summary>
        /// 获取商户缓存项或Null
        /// </summary>
        /// <param name="tenancyName">商户名称</param>
        /// <returns></returns>
        public virtual TenantCacheItem GetOrNull(string tenancyName)
        {
            var tenantId = _cacheManager.GetTenantByNameCache()
                .Get(tenancyName, () => GetTenantOrNull(tenancyName)?.Id);

            if (tenantId == null)
            {
                return null;
            }

            return Get(tenantId.Value);
        }
        /// <summary>
        /// 获取商户缓存项或Null
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <returns></returns>
        public TenantCacheItem GetOrNull(int tenantId)
        {
            return _cacheManager
                .GetTenantCache()
                .Get(
                    tenantId,
                    () =>
                    {
                        var tenant = GetTenantOrNull(tenantId);
                        if (tenant == null)
                        {
                            return null;
                        }

                        return CreateTenantCacheItem(tenant);
                    }
                );
        }
        /// <summary>
        /// 创建商户缓存项
        /// </summary>
        /// <param name="tenant">商户对象</param>
        /// <returns></returns>
        protected virtual TenantCacheItem CreateTenantCacheItem(TTenant tenant)
        {
            return new TenantCacheItem
            {
                Id = tenant.Id,
                Name = tenant.Name,
                TenancyName = tenant.TenancyName,
                EditionId = tenant.EditionId,
                ConnectionString = SimpleStringCipher.Instance.Decrypt(tenant.ConnectionString),
                IsActive = tenant.IsActive
            };
        }
        /// <summary>
        /// 获取商户或Null
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <returns></returns>
        [UnitOfWork]
        protected virtual TTenant GetTenantOrNull(int tenantId)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                return _tenantRepository.FirstOrDefault(tenantId);
            }
        }
        /// <summary>
        /// 获取商户或Null
        /// </summary>
        /// <param name="tenancyName">商户名称</param>
        /// <returns></returns>
        [UnitOfWork]
        protected virtual TTenant GetTenantOrNull(string tenancyName)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                return _tenantRepository.FirstOrDefault(t => t.TenancyName == tenancyName);
            }
        }
        /// <summary>
        /// 处理商户修改事件
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void HandleEvent(EntityChangedEventData<TTenant> eventData)
        {
            var existingCacheItem = _cacheManager.GetTenantCache().GetOrDefault(eventData.Entity.Id);

            _cacheManager
                .GetTenantByNameCache()
                .Remove(
                    existingCacheItem != null
                        ? existingCacheItem.TenancyName
                        : eventData.Entity.TenancyName
                );

            _cacheManager
                .GetTenantCache()
                .Remove(eventData.Entity.Id);
        }
    }
}