using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Runtime.Caching;
using Derrick.Authorization;
using Derrick.Caching.Dto;

namespace Derrick.Caching
{
    /// <summary>
    /// 缓存服务实现
    /// </summary>
    [AbpAuthorize(AppPermissions.Pages_Administration_Host_Maintenance)]
    public class CachingAppService : AbpZeroTemplateAppServiceBase, ICachingAppService
    {
        /// <summary>
        /// 缓存管理引用
        /// </summary>
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheManager">缓存管理引用</param>
        public CachingAppService(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }
        /// <summary>
        /// 获取所有缓存
        /// </summary>
        /// <returns></returns>
        public ListResultDto<CacheDto> GetAllCaches()
        {
            var caches = _cacheManager.GetAllCaches()
                                        .Select(cache => new CacheDto
                                        {
                                            Name = cache.Name
                                        })
                                        .ToList();

            return new ListResultDto<CacheDto>(caches);
        }
        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="input">实体Dto</param>
        /// <returns></returns>
        public async Task ClearCache(EntityDto<string> input)
        {
            var cache = _cacheManager.GetCache(input.Id);
            await cache.ClearAsync();
        }
        /// <summary>
        /// 清除所有缓存
        /// </summary>
        /// <returns></returns>
        public async Task ClearAllCaches()
        {
            var caches = _cacheManager.GetAllCaches();
            foreach (var cache in caches)
            {
                await cache.ClearAsync();
            }
        }
    }
}