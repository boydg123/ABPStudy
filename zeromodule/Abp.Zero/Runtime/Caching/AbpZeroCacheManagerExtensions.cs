using Abp.Application.Editions;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;

namespace Abp.Runtime.Caching
{
    /// <summary>
    /// ABP Zero缓存管理扩展
    /// </summary>
    public static class AbpZeroCacheManagerExtensions
    {
        /// <summary>
        /// 获取用户权限缓存
        /// </summary>
        /// <param name="cacheManager">缓存管理对象</param>
        /// <returns></returns>
        public static ITypedCache<string, UserPermissionCacheItem> GetUserPermissionCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, UserPermissionCacheItem>(UserPermissionCacheItem.CacheStoreName);
        }
        /// <summary>
        /// 获取角色权限缓存
        /// </summary>
        /// <param name="cacheManager">缓存管理对象</param>
        /// <returns></returns>
        public static ITypedCache<string, RolePermissionCacheItem> GetRolePermissionCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, RolePermissionCacheItem>(RolePermissionCacheItem.CacheStoreName);
        }
        /// <summary>
        /// 获取商户功能缓存
        /// </summary>
        /// <param name="cacheManager">缓存管理对象</param>
        /// <returns></returns>
        public static ITypedCache<int, TenantFeatureCacheItem> GetTenantFeatureCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<int, TenantFeatureCacheItem>(TenantFeatureCacheItem.CacheStoreName);
        }
        /// <summary>
        /// 获取版本功能缓存
        /// </summary>
        /// <param name="cacheManager">缓存管理对象</param>
        /// <returns></returns>
        public static ITypedCache<int, EditionfeatureCacheItem> GetEditionFeatureCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<int, EditionfeatureCacheItem>(EditionfeatureCacheItem.CacheStoreName);
        }
    }
}
