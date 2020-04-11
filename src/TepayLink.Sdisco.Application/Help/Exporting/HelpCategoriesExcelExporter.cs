using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Help.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Help.Exporting
{
    public class HelpCategoriesExcelExporter : EpPlusExcelExporterBase, IHelpCategoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HelpCategoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHelpCategoryForViewDto> helpCategories)
        {
            return CreateExcelPackage(
                "HelpCategories.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("HelpCategories"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("CategoryName"),
                        L("Type")
                        );

                    AddObjects(
                        sheet, 2, helpCategories,
                        _ => _.HelpCategory.CategoryName,
                        _ => _.HelpCategory.Type
                        );

					

                });
        }
    }
}
