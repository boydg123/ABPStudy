using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;

namespace Abp.MultiTenancy
{
    /// <summary>
    /// This class handles related events and invalidated tenant feature cache items if needed.
    /// 如果需要，该类处理相关事件和无效的租户特征缓存项。
    /// </summary>
    public class TenantFeatureCacheItemInvalidator :
        IEventHandler<EntityChangedEventData<TenantFeatureSetting>>,
        ITransientDependency
    {
        /// <summary>
        /// 缓存管理引用
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheManager">The cache manager.</param>
        public TenantFeatureCacheItemInvalidator(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// 处理商户功能设置修改事件
        /// </summary>
        /// <param name="eventData"></param>
        public void HandleEvent(EntityChangedEventData<TenantFeatureSetting> eventData)
        {
            _cacheManager.GetTenantFeatureCache().Remove(eventData.Entity.TenantId);
        }
    }
}