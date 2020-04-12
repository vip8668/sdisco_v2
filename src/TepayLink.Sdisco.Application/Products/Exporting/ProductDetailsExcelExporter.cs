using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Products.Exporting
{
    public class ProductDetailsExcelExporter : EpPlusExcelExporterBase, IProductDetailsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductDetailsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductDetailForViewDto> productDetails)
        {
            return CreateExcelPackage(
                "ProductDetails.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ProductDetails"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Title"),
                        L("Order"),
                        L("Description"),
                        L("ThumbImage"),
                        (L("Product")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, productDetails,
                        _ => _.ProductDetail.Title,
                        _ => _.ProductDetail.Order,
                        _ => _.ProductDetail.Description,
                        _ => _.ProductDetail.ThumbImage,
                        _ => _.ProductName
                        );

					

                });
        }
    }
}
