using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Products.Exporting
{
    public class ProductImagesExcelExporter : EpPlusExcelExporterBase, IProductImagesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductImagesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductImageForViewDto> productImages)
        {
            return CreateExcelPackage(
                "ProductImages.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ProductImages"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Url"),
                        L("ImageType"),
                        L("Tag"),
                        L("Title"),
                        (L("Product")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, productImages,
                        _ => _.ProductImage.Url,
                        _ => _.ProductImage.ImageType,
                        _ => _.ProductImage.Tag,
                        _ => _.ProductImage.Title,
                        _ => _.ProductName
                        );

					

                });
        }
    }
}
