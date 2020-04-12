using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Account.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Account.Exporting
{
    public class WalletsExcelExporter : EpPlusExcelExporterBase, IWalletsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public WalletsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetWalletForViewDto> wallets)
        {
            return CreateExcelPackage(
                "Wallets.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Wallets"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Balance"),
                        L("Type"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, wallets,
                        _ => _.Wallet.Balance,
                        _ => _.Wallet.Type,
                        _ => _.UserName
                        );

					

                });
        }
    }
}
