using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Authorization;
using Abp.AutoMapper;
using Derrick.Sessions.Dto;

namespace Derrick.Sessions
{
    /// <summary>
    /// Session服务实现
    /// </summary>
    [AbpAuthorize]
    public class SessionAppService : AbpZeroTemplateAppServiceBase, ISessionAppService
    {
        /// <summary>
        /// 获取当前登录信息
        /// </summary>
        /// <returns></returns>
        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                User = (await GetCurrentUserAsync()).MapTo<UserLoginInfoDto>()
            };

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = (await GetCurrentTenantAsync()).MapTo<TenantLoginInfoDto>();
            }

            return output;
        }
    }
}