using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.AdminConfig.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.AdminConfig.Exporting
{
    public class BankBranchsExcelExporter : EpPlusExcelExporterBase, IBankBranchsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BankBranchsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBankBranchForViewDto> bankBranchs)
        {
            return CreateExcelPackage(
                "BankBranchs.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("BankBranchs"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("BranchName"),
                        L("Address"),
                        L("Order"),
                        (L("Bank")) + L("BankName")
                        );

                    AddObjects(
                        sheet, 2, bankBranchs,
                        _ => _.BankBranch.BranchName,
                        _ => _.BankBranch.Address,
                        _ => _.BankBranch.Order,
                        _ => _.BankBankName
                        );

					

                });
        }
    }
}
