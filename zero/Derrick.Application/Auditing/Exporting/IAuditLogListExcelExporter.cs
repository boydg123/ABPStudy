using System.Collections.Generic;
using Derrick.Auditing.Dto;
using Derrick.Dto;

namespace Derrick.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);
    }
}
