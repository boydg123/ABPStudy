using System.Collections.Generic;
using Derrick.Auditing.Dto;
using Derrick.Dto;

namespace Derrick.Auditing.Exporting
{
    /// <summary>
    /// 审计日志列表Excel导出器
    /// </summary>
    public interface IAuditLogListExcelExporter
    {
        /// <summary>
        /// 将审计日志列表导出到Excel文件
        /// </summary>
        /// <param name="auditLogListDtos">审计日志列表Dto</param>
        /// <returns></returns>
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);
    }
}
