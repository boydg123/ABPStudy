using Abp.Application.Services;
using Derrick.Tenants.Dashboard.Dto;

namespace Derrick.Tenants.Dashboard
{
    public interface ITenantDashboardAppService : IApplicationService
    {
        GetMemberActivityOutput GetMemberActivity();
    }
}
