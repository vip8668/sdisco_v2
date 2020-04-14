using System.Collections.Generic;
using TepayLink.Sdisco.Reports.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Reports.Exporting
{
    public interface IRevenueByMonthsExcelExporter
    {
        FileDto ExportToFile(List<GetRevenueByMonthForViewDto> revenueByMonths);
    }
}