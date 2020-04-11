using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Products.Exporting
{
    public class PlacesExcelExporter : EpPlusExcelExporterBase, IPlacesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PlacesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPlaceForViewDto> places)
        {
            return CreateExcelPackage(
                "Places.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Places"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("DisplayAddress"),
                        L("GoogleAddress"),
                        L("Overview"),
                        L("WhatToExpect"),
                        (L("Detination")) + L("Name"),
                        (L("PlaceCategory")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, places,
                        _ => _.Place.Name,
                        _ => _.Place.DisplayAddress,
                        _ => _.Place.GoogleAddress,
                        _ => _.Place.Overview,
                        _ => _.Place.WhatToExpect,
                        _ => _.DetinationName,
                        _ => _.PlaceCategoryName
                        );

					

                });
        }
    }
}
