using System.Collections.Generic;
using TepayLink.Sdisco.Auditing.Dto;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
