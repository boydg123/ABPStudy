using Abp.Application.Services;
using Derrick.Dto;
using Derrick.Logging.Dto;

namespace Derrick.Logging
{
    /// <summary>
    /// Web日志服务
    /// </summary>
    public interface IWebLogAppService : IApplicationService
    {
        /// <summary>
        /// 获取最新的web日志
        /// </summary>
        /// <returns></returns>
        GetLatestWebLogsOutput GetLatestWebLogs();
        /// <summary>
        /// 下载web日志
        /// </summary>
        /// <returns></returns>
        FileDto DownloadWebLogs();
    }
}
