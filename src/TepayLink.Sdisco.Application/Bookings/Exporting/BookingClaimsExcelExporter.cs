using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Bookings.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Bookings.Exporting
{
    public class BookingClaimsExcelExporter : EpPlusExcelExporterBase, IBookingClaimsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BookingClaimsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBookingClaimForViewDto> bookingClaims)
        {
            return CreateExcelPackage(
                "BookingClaims.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("BookingClaims"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("BookingDetailId"),
                        (L("ClaimReason")) + L("Title")
                        );

                    AddObjects(
                        sheet, 2, bookingClaims,
                        _ => _.BookingClaim.BookingDetailId,
                        _ => _.ClaimReasonTitle
                        );

					

                });
        }
    }
}
