using System.Collections.Generic;
using TepayLink.Sdisco.AdminConfig.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.AdminConfig.Exporting
{
    public interface IBanksExcelExporter
    {
        FileDto ExportToFile(List<GetBankForViewDto> banks);
    }
}