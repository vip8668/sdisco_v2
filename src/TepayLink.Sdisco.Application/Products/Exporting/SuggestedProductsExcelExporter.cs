using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Products.Exporting
{
    public class SuggestedProductsExcelExporter : EpPlusExcelExporterBase, ISuggestedProductsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SuggestedProductsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSuggestedProductForViewDto> suggestedProducts)
        {
            return CreateExcelPackage(
                "SuggestedProducts.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("SuggestedProducts"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        (L("Product")) + L("Name"),
                        (L("Product")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, suggestedProducts,
                        _ => _.ProductName,
                        _ => _.ProductName2
                        );

					

                });
        }
    }
}
