using Abp.Application.Services;
using Derrick.Tenants.Dashboard.Dto;

namespace Derrick.Tenants.Dashboard
{
    /// <summary>
    /// 商户Dashboard服务
    /// </summary>
    public interface ITenantDashboardAppService : IApplicationService
    {
        /// <summary>
        /// 获取激活成员
        /// </summary>
        /// <returns></returns>
        GetMemberActivityOutput GetMemberActivity();
    }
}
