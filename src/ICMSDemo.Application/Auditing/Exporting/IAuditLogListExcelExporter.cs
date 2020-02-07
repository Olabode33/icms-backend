using System.Collections.Generic;
using ICMSDemo.Auditing.Dto;
using ICMSDemo.Dto;

namespace ICMSDemo.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
