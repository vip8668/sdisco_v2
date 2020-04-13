using System.Collections.Generic;
using TepayLink.Sdisco.Account.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Account.Exporting
{
    public interface ITransactionsExcelExporter
    {
        FileDto ExportToFile(List<GetTransactionForViewDto> transactions);
    }
}