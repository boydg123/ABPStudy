using System.Threading.Tasks;
using Abp.Application.Services;
using Derrick.Sessions.Dto;

namespace Derrick.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
