using System.Collections.Generic;
using TepayLink.Sdisco.Cashout.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Cashout.Exporting
{
    public interface ICashoutMethodTypesExcelExporter
    {
        FileDto ExportToFile(List<GetCashoutMethodTypeForViewDto> cashoutMethodTypes);
    }
}