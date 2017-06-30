using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Abp.Runtime.Caching;
using Abp.UI;
using Abp.Web.Models;
using Abp.WebApi.Controllers;

namespace Abp.WebApi.Runtime.Caching
{
    /// <summary>
    /// ABP缓存控制器
    /// </summary>
    [DontWrapResult]
    public class AbpCacheController : AbpApiController
    {
        /// <summary>
        /// 缓存管理引用
        /// </summary>
        private readonly ICacheManager _cacheManager;

        public AbpCacheController(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AjaxResponse> Clear(ClearCacheModel model)
        {
            if (model.Password.IsNullOrEmpty())
            {
                throw new UserFriendlyException("Password can not be null or empty!");
            }

            if (model.Caches.IsNullOrEmpty())
            {
                throw new UserFriendlyException("Caches can not be null or empty!");
            }

            await CheckPassword(model.Password);

            var caches = _cacheManager.GetAllCaches().Where(c => model.Caches.Contains(c.Name));
            foreach (var cache in caches)
            {
                await cache.ClearAsync();
            }

            return new AjaxResponse();
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/AbpCache/ClearAll")]
        public async Task<AjaxResponse> ClearAll(ClearAllCacheModel model)
        {
            if (model.Password.IsNullOrEmpty())
            {
                throw new UserFriendlyException("Password can not be null or empty!");
            }

            await CheckPassword(model.Password);

            var caches = _cacheManager.GetAllCaches();
            foreach (var cache in caches)
            {
                await cache.ClearAsync();
            }

            return new AjaxResponse();
        }

        /// <summary>
        /// 检查密码
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task CheckPassword(string password)
        {
            var actualPassword = await SettingManager.GetSettingValueAsync(ClearCacheSettingNames.Password);
            if (actualPassword != password)
            {
                throw new UserFriendlyException("Password is not correct!");
            }
        }
    }
}
