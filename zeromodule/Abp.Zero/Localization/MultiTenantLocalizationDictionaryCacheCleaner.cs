using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;

namespace Abp.Localization
{
    /// <summary>
    /// Clears related localization cache when a <see cref="ApplicationLanguageText"/> changes.
    /// 当<see cref="ApplicationLanguageText"/>修改时清楚相关的本地化缓存
    /// </summary>
    public class MultiTenantLocalizationDictionaryCacheCleaner : 
        ITransientDependency,
        IEventHandler<EntityChangedEventData<ApplicationLanguageText>>
    {
        /// <summary>
        /// 缓存管理引用
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MultiTenantLocalizationDictionaryCacheCleaner(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// 处理<see cref="ApplicationLanguageText"/>修改事件
        /// </summary>
        /// <param name="eventData"></param>
        public void HandleEvent(EntityChangedEventData<ApplicationLanguageText> eventData)
        {
            _cacheManager
                .GetMultiTenantLocalizationDictionaryCache()
                .Remove(MultiTenantLocalizationDictionaryCacheHelper.CalculateCacheKey(
                    eventData.Entity.TenantId,
                    eventData.Entity.Source,
                    eventData.Entity.LanguageName)
                );
        }
    }
}