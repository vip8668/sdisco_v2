using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Account.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Account.Exporting
{
    public class TransactionsExcelExporter : EpPlusExcelExporterBase, ITransactionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TransactionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTransactionForViewDto> transactions)
        {
            return CreateExcelPackage(
                "Transactions.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Transactions"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("UserId"),
                        L("Amount"),
                        L("Side"),
                        L("TransType"),
                        L("WalletType"),
                        L("BookingDetailId"),
                        L("RefId"),
                        L("Descrition")
                        );

                    AddObjects(
                        sheet, 2, transactions,
                        _ => _.Transaction.UserId,
                        _ => _.Transaction.Amount,
                        _ => _.Transaction.Side,
                        _ => _.Transaction.TransType,
                        _ => _.Transaction.WalletType,
                        _ => _.Transaction.BookingDetailId,
                        _ => _.Transaction.RefId,
                        _ => _.Transaction.Descrition
                        );

					

                });
        }
    }
}
