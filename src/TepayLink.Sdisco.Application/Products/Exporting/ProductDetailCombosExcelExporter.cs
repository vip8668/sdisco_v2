using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Products.Exporting
{
    public class ProductDetailCombosExcelExporter : EpPlusExcelExporterBase, IProductDetailCombosExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductDetailCombosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductDetailComboForViewDto> productDetailCombos)
        {
            return CreateExcelPackage(
                "ProductDetailCombos.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ProductDetailCombos"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("RoomId"),
                        L("Description"),
                        (L("Product")) + L("Name"),
                        (L("ProductDetail")) + L("Title"),
                        (L("Product")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, productDetailCombos,
                        _ => _.ProductDetailCombo.RoomId,
                        _ => _.ProductDetailCombo.Description,
                        _ => _.ProductName,
                        _ => _.ProductDetailTitle,
                        _ => _.ProductName2
                        );

					

                });
        }
    }
}
