using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Authorization.Users;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.IdentityFramework;
using Abp.Localization;
using Abp.Runtime.Caching;
using Abp.Zero;
using Microsoft.AspNet.Identity;

namespace Abp.MultiTenancy
{
    /// <summary>
    /// Tenant manager.Implements domain logic for <see cref="AbpTenant{TUser}"/>.
    /// 商户管理。实现<see cref="AbpTenant{TUser}"/>的域逻辑
    /// </summary>
    /// <typeparam name="TTenant">Type of the application Tenant / 应用程序商户的类型</typeparam>
    /// <typeparam name="TUser">Type of the application User / 应用程序用户的类型</typeparam>
    public abstract class AbpTenantManager<TTenant, TUser> : IDomainService,
        IEventHandler<EntityChangedEventData<TTenant>>,
        IEventHandler<EntityDeletedEventData<Edition>>
        where TTenant : AbpTenant<TUser>
        where TUser : AbpUser<TUser>
    {
        /// <summary>
        /// ABP版本管理引用
        /// </summary>
        public AbpEditionManager EditionManager { get; set; }
        /// <summary>
        /// 本地化管理引用
        /// </summary>
        public ILocalizationManager LocalizationManager { get; set; }
        /// <summary>
        /// 缓存管理引用
        /// </summary>
        public ICacheManager CacheManager { get; set; }
        /// <summary>
        /// 功能管理引用
        /// </summary>
        public IFeatureManager FeatureManager { get; set; }
        /// <summary>
        /// 商户仓储
        /// </summary>
        protected IRepository<TTenant> TenantRepository { get; set; }
        /// <summary>
        /// 商户功能仓储
        /// </summary>
        protected IRepository<TenantFeatureSetting, long> TenantFeatureRepository { get; set; }
        /// <summary>
        /// ABP Zero功能值存储
        /// </summary>
        private readonly IAbpZeroFeatureValueStore _featureValueStore;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tenantRepository">商户仓储</param>
        /// <param name="tenantFeatureRepository">商户功能仓储</param>
        /// <param name="editionManager">ABP版本管理引用</param>
        /// <param name="featureValueStore">ABP Zero功能值存储</param>
        protected AbpTenantManager(
            IRepository<TTenant> tenantRepository, 
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository,
            AbpEditionManager editionManager,
            IAbpZeroFeatureValueStore featureValueStore)
        {
            _featureValueStore = featureValueStore;
            TenantRepository = tenantRepository;
            TenantFeatureRepository = tenantFeatureRepository;
            EditionManager = editionManager;
            LocalizationManager = NullLocalizationManager.Instance;
        }
        /// <summary>
        /// 商户列表
        /// </summary>
        public virtual IQueryable<TTenant> Tenants { get { return TenantRepository.GetAll(); } }
        /// <summary>
        /// 创建商户
        /// </summary>
        /// <param name="tenant">商户对象</param>
        /// <returns></returns>
        public virtual async Task<IdentityResult> CreateAsync(TTenant tenant)
        {
            if (await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenant.TenancyName) != null)
            {
                return AbpIdentityResult.Failed(string.Format(L("TenancyNameIsAlreadyTaken"), tenant.TenancyName));
            }

            var validationResult = await ValidateTenantAsync(tenant);
            if (!validationResult.Succeeded)
            {
                return validationResult;
            }

            await TenantRepository.InsertAsync(tenant);
            return IdentityResult.Success;
        }
        /// <summary>
        /// 更新商户
        /// </summary>
        /// <param name="tenant">商户对象</param>
        /// <returns></returns>
        public async Task<IdentityResult> UpdateAsync(TTenant tenant)
        {
            if (await TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenant.TenancyName && t.Id != tenant.Id) != null)
            {
                return AbpIdentityResult.Failed(string.Format(L("TenancyNameIsAlreadyTaken"), tenant.TenancyName));
            }

            await TenantRepository.UpdateAsync(tenant);
            return IdentityResult.Success;
        }
        /// <summary>
        /// 功过ID查找商户
        /// </summary>
        /// <param name="id">商户ID</param>
        /// <returns></returns>
        public virtual async Task<TTenant> FindByIdAsync(int id)
        {
            return await TenantRepository.FirstOrDefaultAsync(id);
        }
        /// <summary>
        /// 通过商户获取ID，如果没找到则抛出异常
        /// </summary>
        /// <param name="id">商户ID</param>
        /// <returns></returns>
        public virtual async Task<TTenant> GetByIdAsync(int id)
        {
            var tenant = await FindByIdAsync(id);
            if (tenant == null)
            {
                throw new AbpException("There is no tenant with id: " + id);
            }

            return tenant;
        }
        /// <summary>
        /// 通过商户名称查找商户
        /// </summary>
        /// <param name="tenancyName">商户名称</param>
        /// <returns></returns>
        public virtual Task<TTenant> FindByTenancyNameAsync(string tenancyName)
        {
            return TenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
        }
        /// <summary>
        /// 删除商户
        /// </summary>
        /// <param name="tenant">商户对象</param>
        /// <returns></returns>
        public virtual async Task<IdentityResult> DeleteAsync(TTenant tenant)
        {
            await TenantRepository.DeleteAsync(tenant);
            return IdentityResult.Success;
        }
        /// <summary>
        /// 获取功能值或Null
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <param name="featureName">功能名称</param>
        /// <returns></returns>
        public Task<string> GetFeatureValueOrNullAsync(int tenantId, string featureName)
        {
            return _featureValueStore.GetValueOrNullAsync(tenantId, featureName);
        }
        /// <summary>
        /// 获取功能值列表
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <returns></returns>
        public virtual async Task<IReadOnlyList<NameValue>> GetFeatureValuesAsync(int tenantId)
        {
            var values = new List<NameValue>();

            foreach (var feature in FeatureManager.GetAll())
            {
                values.Add(new NameValue(feature.Name, await GetFeatureValueOrNullAsync(tenantId, feature.Name) ?? feature.DefaultValue));
            }

            return values;
        }
        /// <summary>
        /// 设置功能值
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <param name="values">功能值列表</param>
        /// <returns></returns>
        public virtual async Task SetFeatureValuesAsync(int tenantId, params NameValue[] values)
        {
            if (values.IsNullOrEmpty())
            {
                return;
            }

            foreach (var value in values)
            {
                await SetFeatureValueAsync(tenantId, value.Name, value.Value);
            }
        }
        /// <summary>
        /// 设置功能值
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <param name="featureName">功能名称</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task SetFeatureValueAsync(int tenantId, string featureName, string value)
        {
            await SetFeatureValueAsync(await GetByIdAsync(tenantId), featureName, value);
        }
        /// <summary>
        /// 设置功能值
        /// </summary>
        /// <param name="tenant">商户对象</param>
        /// <param name="featureName">功能名称</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task SetFeatureValueAsync(TTenant tenant, string featureName, string value)
        {
            //No need to change if it's already equals to the current value
            if (await GetFeatureValueOrNullAsync(tenant.Id, featureName) == value)
            {
                return;
            }

            //Get the current feature setting
            var currentSetting = await TenantFeatureRepository.FirstOrDefaultAsync(f => f.TenantId == tenant.Id && f.Name == featureName);

            //Get the feature
            var feature = FeatureManager.GetOrNull(featureName);
            if (feature == null)
            {
                if (currentSetting != null)
                {
                    await TenantFeatureRepository.DeleteAsync(currentSetting);
                }

                return;
            }

            //Determine default value
            var defaultValue = tenant.EditionId.HasValue
                ? (await EditionManager.GetFeatureValueOrNullAsync(tenant.EditionId.Value, featureName) ?? feature.DefaultValue)
                : feature.DefaultValue;

            //No need to store value if it's default
            if (value == defaultValue)
            {
                if (currentSetting != null)
                {
                    await TenantFeatureRepository.DeleteAsync(currentSetting);
                }

                return;
            }

            //Insert/update the feature value
            if (currentSetting == null)
            {
                await TenantFeatureRepository.InsertAsync(new TenantFeatureSetting(tenant.Id, featureName, value));
            }
            else
            {
                currentSetting.Value = value;
            }
        }

        /// <summary>
        /// Resets all custom feature settings for a tenant.Tenant will have features according to it's edition.
        /// 为商户重置所有自定义功能。商户将拥有其版本的特点
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        public async Task ResetAllFeaturesAsync(int tenantId)
        {
            await TenantFeatureRepository.DeleteAsync(f => f.TenantId == tenantId);
        }
        /// <summary>
        /// 验证商户
        /// </summary>
        /// <param name="tenant">商户对象</param>
        /// <returns></returns>
        protected virtual async Task<IdentityResult> ValidateTenantAsync(TTenant tenant)
        {
            var nameValidationResult = await ValidateTenancyNameAsync(tenant.TenancyName);
            if (!nameValidationResult.Succeeded)
            {
                return nameValidationResult;
            }

            return IdentityResult.Success;
        }
        /// <summary>
        /// 根据商户名称验证商户
        /// </summary>
        /// <param name="tenancyName">商户名</param>
        /// <returns></returns>
        protected virtual async Task<IdentityResult> ValidateTenancyNameAsync(string tenancyName)
        {
            if (!Regex.IsMatch(tenancyName, AbpTenant<TUser>.TenancyNameRegex))
            {
                return AbpIdentityResult.Failed(L("InvalidTenancyName"));
            }

            return IdentityResult.Success;
        }
        /// <summary>
        /// 获取本地化字符串
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        private string L(string name)
        {
            return LocalizationManager.GetString(AbpZeroConsts.LocalizationSourceName, name);
        }
        /// <summary>
        /// 处理商户修改事件
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void HandleEvent(EntityChangedEventData<TTenant> eventData)
        {
            if (eventData.Entity.IsTransient())
            {
                return;
            }

            CacheManager.GetTenantFeatureCache().Remove(eventData.Entity.Id);
        }
        /// <summary>
        /// 处理商户删除事件
        /// </summary>
        /// <param name="eventData">事件数据</param>
        [UnitOfWork]
        public virtual void HandleEvent(EntityDeletedEventData<Edition> eventData)
        {
            var relatedTenants = TenantRepository.GetAllList(t => t.EditionId == eventData.Entity.Id);
            foreach (var relatedTenant in relatedTenants)
            {
                relatedTenant.EditionId = null;
            }
        }
    }
}