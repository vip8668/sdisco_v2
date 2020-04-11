using System.Collections.Generic;
using TepayLink.Sdisco.AdminConfig.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.AdminConfig.Exporting
{
    public interface IBankBranchsExcelExporter
    {
        FileDto ExportToFile(List<GetBankBranchForViewDto> bankBranchs);
    }
}