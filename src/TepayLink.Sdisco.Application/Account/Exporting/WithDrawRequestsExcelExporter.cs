using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Account.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Account.Exporting
{
    public class WithDrawRequestsExcelExporter : EpPlusExcelExporterBase, IWithDrawRequestsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public WithDrawRequestsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetWithDrawRequestForViewDto> withDrawRequests)
        {
            return CreateExcelPackage(
                "WithDrawRequests.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("WithDrawRequests"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("UserId"),
                        L("Amount"),
                        L("Status"),
                        L("TransactionId"),
                        L("BankAccountId")
                        );

                    AddObjects(
                        sheet, 2, withDrawRequests,
                        _ => _.WithDrawRequest.UserId,
                        _ => _.WithDrawRequest.Amount,
                        _ => _.WithDrawRequest.Status,
                        _ => _.WithDrawRequest.TransactionId,
                        _ => _.WithDrawRequest.BankAccountId
                        );

					

                });
        }
    }
}
