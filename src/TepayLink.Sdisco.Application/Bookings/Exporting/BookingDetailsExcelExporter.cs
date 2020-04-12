using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Bookings.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Bookings.Exporting
{
    public class BookingDetailsExcelExporter : EpPlusExcelExporterBase, IBookingDetailsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BookingDetailsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBookingDetailForViewDto> bookingDetails)
        {
            return CreateExcelPackage(
                "BookingDetails.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("BookingDetails"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("BookingId"),
                        L("StartDate"),
                        L("EndDate"),
                        L("TripLength"),
                        L("Status"),
                        L("ProductScheduleId"),
                        L("Quantity"),
                        L("Amount"),
                        L("Fee"),
                        L("HostPaymentStatus"),
                        L("HostUserId"),
                        L("BookingUserId"),
                        L("IsDone"),
                        L("AffiliateUserId"),
                        L("RoomId"),
                        L("Note"),
                        L("CancelDate"),
                        L("RefundAmount"),
                        (L("Product")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, bookingDetails,
                        _ => _.BookingDetail.BookingId,
                        _ => _timeZoneConverter.Convert(_.BookingDetail.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.BookingDetail.EndDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.BookingDetail.TripLength, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.BookingDetail.Status,
                        _ => _.BookingDetail.ProductScheduleId,
                        _ => _.BookingDetail.Quantity,
                        _ => _.BookingDetail.Amount,
                        _ => _.BookingDetail.Fee,
                        _ => _.BookingDetail.HostPaymentStatus,
                        _ => _.BookingDetail.HostUserId,
                        _ => _.BookingDetail.BookingUserId,
                        _ => _.BookingDetail.IsDone,
                        _ => _.BookingDetail.AffiliateUserId,
                        _ => _.BookingDetail.RoomId,
                        _ => _.BookingDetail.Note,
                        _ => _timeZoneConverter.Convert(_.BookingDetail.CancelDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.BookingDetail.RefundAmount,
                        _ => _.ProductName
                        );

					var startDateColumn = sheet.Column(2);
                    startDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					startDateColumn.AutoFit();
					var endDateColumn = sheet.Column(3);
                    endDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					endDateColumn.AutoFit();
					var tripLengthColumn = sheet.Column(4);
                    tripLengthColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					tripLengthColumn.AutoFit();
					var cancelDateColumn = sheet.Column(17);
                    cancelDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					cancelDateColumn.AutoFit();
					

                });
        }
    }
}
