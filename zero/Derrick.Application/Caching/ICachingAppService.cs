using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Derrick.Caching.Dto;

namespace Derrick.Caching
{
    /// <summary>
    /// 缓存服务
    /// </summary>
    public interface ICachingAppService : IApplicationService
    {
        /// <summary>
        /// 获取所有缓存
        /// </summary>
        /// <returns></returns>
        ListResultDto<CacheDto> GetAllCaches();
        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="input">实体Dto</param>
        /// <returns></returns>
        Task ClearCache(EntityDto<string> input);
        /// <summary>
        /// 清除所有缓存
        /// </summary>
        /// <returns></returns>
        Task ClearAllCaches();
    }
}
