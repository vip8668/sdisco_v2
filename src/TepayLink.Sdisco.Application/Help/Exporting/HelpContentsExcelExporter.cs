using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Help.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Help.Exporting
{
    public class HelpContentsExcelExporter : EpPlusExcelExporterBase, IHelpContentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HelpContentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHelpContentForViewDto> helpContents)
        {
            return CreateExcelPackage(
                "HelpContents.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("HelpContents"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Question"),
                        L("Answer"),
                        (L("HelpCategory")) + L("CategoryName")
                        );

                    AddObjects(
                        sheet, 2, helpContents,
                        _ => _.HelpContent.Question,
                        _ => _.HelpContent.Answer,
                        _ => _.HelpCategoryCategoryName
                        );

					

                });
        }
    }
}
