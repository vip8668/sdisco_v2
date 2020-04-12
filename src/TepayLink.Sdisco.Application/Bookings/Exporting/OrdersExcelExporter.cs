using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Bookings.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Bookings.Exporting
{
    public class OrdersExcelExporter : EpPlusExcelExporterBase, IOrdersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OrdersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOrderForViewDto> orders)
        {
            return CreateExcelPackage(
                "Orders.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Orders"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("OrderCode"),
                        L("OrderType"),
                        L("Amount"),
                        L("Note"),
                        L("Status"),
                        L("OrderRef"),
                        L("UserId"),
                        L("BankCode"),
                        L("CardId"),
                        L("CardNumber"),
                        L("Currency"),
                        L("IssueDate"),
                        L("NameOnCard"),
                        L("TransactionId"),
                        L("BookingId")
                        );

                    AddObjects(
                        sheet, 2, orders,
                        _ => _.Order.OrderCode,
                        _ => _.Order.OrderType,
                        _ => _.Order.Amount,
                        _ => _.Order.Note,
                        _ => _.Order.Status,
                        _ => _.Order.OrderRef,
                        _ => _.Order.UserId,
                        _ => _.Order.BankCode,
                        _ => _.Order.CardId,
                        _ => _.Order.CardNumber,
                        _ => _.Order.Currency,
                        _ => _.Order.IssueDate,
                        _ => _.Order.NameOnCard,
                        _ => _.Order.TransactionId,
                        _ => _.Order.BookingId
                        );

					

                });
        }
    }
}
