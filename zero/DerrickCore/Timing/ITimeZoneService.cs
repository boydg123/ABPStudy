using System.Threading.Tasks;
using Abp.Configuration;

namespace Derrick.Timing
{
    public interface ITimeZoneService
    {
        Task<string> GetDefaultTimezoneAsync(SettingScopes scope, int? tenantId);
    }
}
