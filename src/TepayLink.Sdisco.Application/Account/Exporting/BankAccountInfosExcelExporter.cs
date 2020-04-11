using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Account.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Account.Exporting
{
    public class BankAccountInfosExcelExporter : EpPlusExcelExporterBase, IBankAccountInfosExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BankAccountInfosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBankAccountInfoForViewDto> bankAccountInfos)
        {
            return CreateExcelPackage(
                "BankAccountInfos.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("BankAccountInfos"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("AccountName"),
                        L("AccountNo"),
                        (L("Bank")) + L("BankName"),
                        (L("BankBranch")) + L("BranchName"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, bankAccountInfos,
                        _ => _.BankAccountInfo.AccountName,
                        _ => _.BankAccountInfo.AccountNo,
                        _ => _.BankBankName,
                        _ => _.BankBranchBranchName,
                        _ => _.UserName
                        );

					

                });
        }
    }
}
