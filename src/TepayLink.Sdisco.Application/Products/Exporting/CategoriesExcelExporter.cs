using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Products.Exporting
{
    public class CategoriesExcelExporter : EpPlusExcelExporterBase, ICategoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CategoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCategoryForViewDto> categories)
        {
            return CreateExcelPackage(
                "Categories.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Categories"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Image"),
                        L("Icon"),
                        L("ProductType"),
                        L("Order")
                        );

                    AddObjects(
                        sheet, 2, categories,
                        _ => _.Category.Name,
                        _ => _.Category.Image,
                        _ => _.Category.Icon,
                        _ => _.Category.ProductType,
                        _ => _.Category.Order
                        );

					

                });
        }
    }
}
