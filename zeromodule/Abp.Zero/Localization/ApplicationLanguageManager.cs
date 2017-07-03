using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;

namespace Abp.Localization
{
    /// <summary>
    /// Manages host and tenant languages.
    /// 管理宿主和商户语言
    /// </summary>
    public class ApplicationLanguageManager :
        IApplicationLanguageManager,
        IEventHandler<EntityChangedEventData<ApplicationLanguage>>,
        ISingletonDependency
    {
        /// <summary>
        /// 语言的缓存名.
        /// </summary>
        public const string CacheName = "AbpZeroLanguages";
        /// <summary>
        /// 语言缓存列表
        /// </summary>
        private ITypedCache<int, Dictionary<string, ApplicationLanguage>> LanguageListCache
        {
            get { return _cacheManager.GetCache<int, Dictionary<string, ApplicationLanguage>>(CacheName); }
        }

        /// <summary>
        /// 语言仓储
        /// </summary>
        private readonly IRepository<ApplicationLanguage> _languageRepository;
        /// <summary>
        /// 缓存管理引用
        /// </summary>
        private readonly ICacheManager _cacheManager;
        /// <summary>
        /// 工作单元引用
        /// </summary>
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        /// <summary>
        /// 设置管理引用
        /// </summary>
        private readonly ISettingManager _settingManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApplicationLanguageManager(
            IRepository<ApplicationLanguage> languageRepository,
            ICacheManager cacheManager,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager)
        {
            _languageRepository = languageRepository;
            _cacheManager = cacheManager;
            _unitOfWorkManager = unitOfWorkManager;
            _settingManager = settingManager;
        }

        /// <summary>
        /// Gets list of all languages available to given tenant (or null for host)
        /// 为指定上火获取所有可用的语言列表(宿主商户可为Null)
        /// </summary>
        /// <param name="tenantId">TenantId or null for host / 商户ID或Null(宿主商户)</param>
        public async Task<IReadOnlyList<ApplicationLanguage>> GetLanguagesAsync(int? tenantId)
        {
            return (await GetLanguageDictionary(tenantId)).Values.ToImmutableList();
        }

        /// <summary>
        /// 添加一个新语言
        /// </summary>
        /// <param name="language">语言信息</param>
        [UnitOfWork]
        public virtual async Task AddAsync(ApplicationLanguage language)
        {
            if ((await GetLanguagesAsync(language.TenantId)).Any(l => l.Name == language.Name))
            {
                throw new AbpException("There is already a language with name = " + language.Name);
            }

            using (_unitOfWorkManager.Current.SetTenantId(language.TenantId))
            {
                await _languageRepository.InsertAsync(language);
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 删除语言.
        /// </summary>
        /// <param name="tenantId">Tenant Id or null for host. / 商户ID或Null(商户为宿主商户)</param>
        /// <param name="languageName">Name of the language. / 语言的名称</param>
        [UnitOfWork]
        public virtual async Task RemoveAsync(int? tenantId, string languageName)
        {
            var currentLanguage = (await GetLanguagesAsync(tenantId)).FirstOrDefault(l => l.Name == languageName);
            if (currentLanguage == null)
            {
                return;
            }

            if (currentLanguage.TenantId == null && tenantId != null)
            {
                throw new AbpException("Can not delete a host language from tenant!");
            }

            using (_unitOfWorkManager.Current.SetTenantId(currentLanguage.TenantId))
            {
                await _languageRepository.DeleteAsync(currentLanguage.Id);
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 更新语言.
        /// </summary>
        [UnitOfWork]
        public virtual async Task UpdateAsync(int? tenantId, ApplicationLanguage language)
        {
            var existingLanguageWithSameName = (await GetLanguagesAsync(language.TenantId)).FirstOrDefault(l => l.Name == language.Name);
            if (existingLanguageWithSameName != null)
            {
                if (existingLanguageWithSameName.Id != language.Id)
                {
                    throw new AbpException("There is already a language with name = " + language.Name);
                }
            }

            if (language.TenantId == null && tenantId != null)
            {
                throw new AbpException("Can not update a host language from tenant");
            }

            using (_unitOfWorkManager.Current.SetTenantId(language.TenantId))
            {
                await _languageRepository.UpdateAsync(language);
                await _unitOfWorkManager.Current.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Gets the default language or null for a tenant or the host.
        /// 获取默认语言或Null(商户是宿主商户)
        /// </summary>
        /// <param name="tenantId">Tenant Id of null for host / 商户ID或Noll(宿主商户)</param>
        public async Task<ApplicationLanguage> GetDefaultLanguageOrNullAsync(int? tenantId)
        {
            var defaultLanguageName = tenantId.HasValue
                ? await _settingManager.GetSettingValueForTenantAsync(LocalizationSettingNames.DefaultLanguage, tenantId.Value)
                : await _settingManager.GetSettingValueForApplicationAsync(LocalizationSettingNames.DefaultLanguage);

            return (await GetLanguagesAsync(tenantId)).FirstOrDefault(l => l.Name == defaultLanguageName);
        }

        /// <summary>
        /// Sets the default language for a tenant or the host.
        /// 为商户或宿主设置默认语言
        /// </summary>
        /// <param name="tenantId">Tenant Id of null for host / 商户ID或Noll(宿主商户)</param>
        /// <param name="languageName">Name of the language. / 语言名称</param>
        public async Task SetDefaultLanguageAsync(int? tenantId, string languageName)
        {
            var cultureInfo = CultureInfo.GetCultureInfo(languageName);
            if (tenantId.HasValue)
            {
                await _settingManager.ChangeSettingForTenantAsync(tenantId.Value, LocalizationSettingNames.DefaultLanguage, cultureInfo.Name);
            }
            else
            {
                await _settingManager.ChangeSettingForApplicationAsync(LocalizationSettingNames.DefaultLanguage, cultureInfo.Name);
            }
        }
        /// <summary>
        /// 处理应用程序修改事件
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void HandleEvent(EntityChangedEventData<ApplicationLanguage> eventData)
        {
            LanguageListCache.Remove(eventData.Entity.TenantId ?? 0);

            //Also invalidate the language script cache
            _cacheManager.GetCache("AbpLocalizationScripts").Clear();
        }
        /// <summary>
        /// 获取语言字典
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <returns></returns>
        private async Task<Dictionary<string, ApplicationLanguage>> GetLanguageDictionary(int? tenantId)
        {
            //Creates a copy of the cached dictionary (to not modify it)
            var languageDictionary = new Dictionary<string, ApplicationLanguage>(await GetLanguageDictionaryFromCacheAsync(null));

            if (tenantId == null)
            {
                return languageDictionary;
            }

            //Override tenant languages
            foreach (var tenantLanguage in await GetLanguageDictionaryFromCacheAsync(tenantId.Value))
            {
                languageDictionary[tenantLanguage.Key] = tenantLanguage.Value;
            }

            return languageDictionary;
        }
        /// <summary>
        /// 从缓存获取语言字典
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <returns></returns>
        private Task<Dictionary<string, ApplicationLanguage>> GetLanguageDictionaryFromCacheAsync(int? tenantId)
        {
            return LanguageListCache.GetAsync(tenantId ?? 0, () => GetLanguagesFromDatabaseAsync(tenantId));
        }
        /// <summary>
        /// 从数据库获取语言
        /// </summary>
        /// <param name="tenantId">商户ID</param>
        /// <returns></returns>
        [UnitOfWork]
        protected virtual async Task<Dictionary<string, ApplicationLanguage>> GetLanguagesFromDatabaseAsync(int? tenantId)
        {
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                return (await _languageRepository.GetAllListAsync()).ToDictionary(l => l.Name);
            }
        }
    }
}