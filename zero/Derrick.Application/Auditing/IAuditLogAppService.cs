using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Derrick.Auditing.Dto;
using Derrick.Dto;

namespace Derrick.Auditing
{
    /// <summary>
    /// 审计日志APP服务
    /// </summary>
    public interface IAuditLogAppService : IApplicationService
    {
        /// <summary>
        /// 获取审计日志列表(带分页)
        /// </summary>
        /// <param name="input">审计日志输入对象</param>
        /// <returns></returns>
        Task<PagedResultDto<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input);

        /// <summary>
        /// 获取导出到Excel审计日志
        /// </summary>
        /// <param name="input">审计日志输入对象</param>
        /// <returns></returns>
        Task<FileDto> GetAuditLogsToExcel(GetAuditLogsInput input);
    }
}