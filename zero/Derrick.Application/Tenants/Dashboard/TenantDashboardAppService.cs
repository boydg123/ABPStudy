using System.Linq;
using Abp;
using Abp.Authorization;
using Derrick.Authorization;
using Derrick.Tenants.Dashboard.Dto;

namespace Derrick.Tenants.Dashboard
{
    /// <summary>
    /// 商户Dashboard服务实现
    /// </summary>
    [AbpAuthorize(AppPermissions.Pages_Tenant_Dashboard)]
    public class TenantDashboardAppService : AbpZeroTemplateAppServiceBase, ITenantDashboardAppService
    {
        /// <summary>
        /// 获取激活成员
        /// </summary>
        /// <returns></returns>
        public GetMemberActivityOutput GetMemberActivity()
        {
            //Generating some random data. We could get numbers from database...
            return new GetMemberActivityOutput
                   {
                       TotalMembers = Enumerable.Range(0, 13).Select(i => RandomHelper.GetRandom(15, 40)).ToList(),
                       NewMembers = Enumerable.Range(0, 13).Select(i => RandomHelper.GetRandom(3, 15)).ToList()
                   };
        }
    }
}