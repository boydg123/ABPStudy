using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Derrick.Authorization.Users.Dto;

namespace Derrick.Authorization.Users
{
    /// <summary>
    /// 用户登录APP服务
    /// </summary>
    public interface IUserLoginAppService : IApplicationService
    {
        /// <summary>
        /// 获取最新的用户登录尝试
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<UserLoginAttemptDto>> GetRecentUserLoginAttempts();
    }
}
