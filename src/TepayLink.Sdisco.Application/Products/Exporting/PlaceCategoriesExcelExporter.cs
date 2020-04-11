using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Products.Exporting
{
    public class PlaceCategoriesExcelExporter : EpPlusExcelExporterBase, IPlaceCategoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PlaceCategoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPlaceCategoryForViewDto> placeCategories)
        {
            return CreateExcelPackage(
                "PlaceCategories.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("PlaceCategories"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Image"),
                        L("Icon")
                        );

                    AddObjects(
                        sheet, 2, placeCategories,
                        _ => _.PlaceCategory.Name,
                        _ => _.PlaceCategory.Image,
                        _ => _.PlaceCategory.Icon
                        );

					

                });
        }
    }
}
