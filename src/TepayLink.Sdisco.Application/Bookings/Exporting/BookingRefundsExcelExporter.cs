using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Bookings.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Bookings.Exporting
{
    public class BookingRefundsExcelExporter : EpPlusExcelExporterBase, IBookingRefundsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BookingRefundsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBookingRefundForViewDto> bookingRefunds)
        {
            return CreateExcelPackage(
                "BookingRefunds.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("BookingRefunds"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("BookingDetailId"),
                        L("RefundMethodId"),
                        L("Description"),
                        L("Status"),
                        L("Amount")
                        );

                    AddObjects(
                        sheet, 2, bookingRefunds,
                        _ => _.BookingRefund.BookingDetailId,
                        _ => _.BookingRefund.RefundMethodId,
                        _ => _.BookingRefund.Description,
                        _ => _.BookingRefund.Status,
                        _ => _.BookingRefund.Amount
                        );

					

                });
        }
    }
}
