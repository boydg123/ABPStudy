using System.Threading.Tasks;
using Abp.Application.Services;
using Derrick.Configuration.Host.Dto;

namespace Derrick.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
