using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.KOL.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.KOL.Exporting
{
    public class ShareTransactionsExcelExporter : EpPlusExcelExporterBase, IShareTransactionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ShareTransactionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetShareTransactionForViewDto> shareTransactions)
        {
            return CreateExcelPackage(
                "ShareTransactions.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ShareTransactions"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("UserId"),
                        L("Type"),
                        L("IP"),
                        L("Point"),
                        L("ProductId")
                        );

                    AddObjects(
                        sheet, 2, shareTransactions,
                        _ => _.ShareTransaction.UserId,
                        _ => _.ShareTransaction.Type,
                        _ => _.ShareTransaction.IP,
                        _ => _.ShareTransaction.Point,
                        _ => _.ShareTransaction.ProductId
                        );

					

                });
        }
    }
}
