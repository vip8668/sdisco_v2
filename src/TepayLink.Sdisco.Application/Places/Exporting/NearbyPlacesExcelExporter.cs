using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Places.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Places.Exporting
{
    public class NearbyPlacesExcelExporter : EpPlusExcelExporterBase, INearbyPlacesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public NearbyPlacesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetNearbyPlaceForViewDto> nearbyPlaces)
        {
            return CreateExcelPackage(
                "NearbyPlaces.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("NearbyPlaces"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Description"),
                        (L("Place")) + L("Name"),
                        (L("Place")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, nearbyPlaces,
                        _ => _.NearbyPlace.Description,
                        _ => _.PlaceName,
                        _ => _.PlaceName2
                        );

					

                });
        }
    }
}
