using System.Threading.Tasks;
using Abp.Application.Services;
using Derrick.Sessions.Dto;

namespace Derrick.Sessions
{
    /// <summary>
    /// Session 服务
    /// </summary>
    public interface ISessionAppService : IApplicationService
    {
        /// <summary>
        /// 获取当前登录信息
        /// </summary>
        /// <returns></returns>
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
