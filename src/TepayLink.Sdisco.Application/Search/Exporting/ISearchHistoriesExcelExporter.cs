using System.Collections.Generic;
using TepayLink.Sdisco.Search.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Search.Exporting
{
    public interface ISearchHistoriesExcelExporter
    {
        FileDto ExportToFile(List<GetSearchHistoryForViewDto> searchHistories);
    }
}