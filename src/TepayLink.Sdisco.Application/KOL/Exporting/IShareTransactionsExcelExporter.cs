using System.Collections.Generic;
using TepayLink.Sdisco.KOL.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.KOL.Exporting
{
    public interface IShareTransactionsExcelExporter
    {
        FileDto ExportToFile(List<GetShareTransactionForViewDto> shareTransactions);
    }
}