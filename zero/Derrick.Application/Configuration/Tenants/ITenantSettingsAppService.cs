using System.Threading.Tasks;
using Abp.Application.Services;
using Derrick.Configuration.Tenants.Dto;

namespace Derrick.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);
    }
}
