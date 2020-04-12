using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Search.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Search.Exporting
{
    public class SearchHistoriesExcelExporter : EpPlusExcelExporterBase, ISearchHistoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SearchHistoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSearchHistoryForViewDto> searchHistories)
        {
            return CreateExcelPackage(
                "SearchHistories.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("SearchHistories"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("UserId"),
                        L("Keyword"),
                        L("Type")
                        );

                    AddObjects(
                        sheet, 2, searchHistories,
                        _ => _.SearchHistory.UserId,
                        _ => _.SearchHistory.Keyword,
                        _ => _.SearchHistory.Type
                        );

					

                });
        }
    }
}
