using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;

namespace Abp.Authorization.Users
{
    /// <summary>
    /// ABP的用户权限缓存项
    /// </summary>
    public class AbpUserPermissionCacheItemInvalidator :
        IEventHandler<EntityChangedEventData<UserPermissionSetting>>,
        IEventHandler<EntityChangedEventData<UserRole>>,
        IEventHandler<EntityDeletedEventData<AbpUserBase>>,

        ITransientDependency
    {
        /// <summary>
        /// 缓存管理引用
        /// </summary>
        private readonly ICacheManager _cacheManager;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheManager">缓存管理引用</param>
        public AbpUserPermissionCacheItemInvalidator(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }
        /// <summary>
        /// 处理用户权限设置修改事件
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void HandleEvent(EntityChangedEventData<UserPermissionSetting> eventData)
        {
            var cacheKey = eventData.Entity.UserId + "@" + (eventData.Entity.TenantId ?? 0);
            _cacheManager.GetUserPermissionCache().Remove(cacheKey);
        }
        /// <summary>
        /// 处理用户角色修改事件
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void HandleEvent(EntityChangedEventData<UserRole> eventData)
        {
            var cacheKey = eventData.Entity.UserId + "@" + (eventData.Entity.TenantId ?? 0);
            _cacheManager.GetUserPermissionCache().Remove(cacheKey);
        }
        /// <summary>
        /// 处理用户删除事件
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void HandleEvent(EntityDeletedEventData<AbpUserBase> eventData)
        {
            var cacheKey = eventData.Entity.Id + "@" + (eventData.Entity.TenantId ?? 0);
            _cacheManager.GetUserPermissionCache().Remove(cacheKey);
        }
    }
}