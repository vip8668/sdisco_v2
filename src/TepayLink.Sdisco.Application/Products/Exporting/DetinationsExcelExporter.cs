using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Products.Exporting
{
    public class DetinationsExcelExporter : EpPlusExcelExporterBase, IDetinationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DetinationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDetinationForViewDto> detinations)
        {
            return CreateExcelPackage(
                "Detinations.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Detinations"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Image"),
                        L("Name"),
                        L("Status"),
                        L("IsTop"),
                        L("BookingCount")
                        );

                    AddObjects(
                        sheet, 2, detinations,
                        _ => _.Detination.Image,
                        _ => _.Detination.Name,
                        _ => _.Detination.Status,
                        _ => _.Detination.IsTop,
                        _ => _.Detination.BookingCount
                        );

					

                });
        }
    }
}
