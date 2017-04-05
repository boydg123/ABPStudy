using Abp.Application.Services;
using Derrick.Dto;
using Derrick.Logging.Dto;

namespace Derrick.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
