using System.Collections.Generic;
using TepayLink.Sdisco.Cashout.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Cashout.Exporting
{
    public interface IUserDefaultCashoutMethodTypesExcelExporter
    {
        FileDto ExportToFile(List<GetUserDefaultCashoutMethodTypeForViewDto> userDefaultCashoutMethodTypes);
    }
}