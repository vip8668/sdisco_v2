using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Bookings.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Bookings.Exporting
{
    public class BookingsExcelExporter : EpPlusExcelExporterBase, IBookingsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BookingsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBookingForViewDto> bookings)
        {
            return CreateExcelPackage(
                "Bookings.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Bookings"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("StartDate"),
                        L("EndDate"),
                        L("TripLength"),
                        L("Status"),
                        L("Quantity"),
                        L("Amount"),
                        L("Fee"),
                        L("Note"),
                        L("GuestInfo"),
                        L("CouponCode"),
                        L("BonusAmount"),
                        L("Contact"),
                        L("CouponId"),
                        L("TotalAmount"),
                        (L("Product")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, bookings,
                        _ => _timeZoneConverter.Convert(_.Booking.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Booking.EndDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Booking.TripLength,
                        _ => _.Booking.Status,
                        _ => _.Booking.Quantity,
                        _ => _.Booking.Amount,
                        _ => _.Booking.Fee,
                        _ => _.Booking.Note,
                        _ => _.Booking.GuestInfo,
                        _ => _.Booking.CouponCode,
                        _ => _.Booking.BonusAmount,
                        _ => _.Booking.Contact,
                        _ => _.Booking.CouponId,
                        _ => _.Booking.TotalAmount,
                        _ => _.ProductName
                        );

					var startDateColumn = sheet.Column(1);
                    startDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					startDateColumn.AutoFit();
					var endDateColumn = sheet.Column(2);
                    endDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					endDateColumn.AutoFit();
					

                });
        }
    }
}
