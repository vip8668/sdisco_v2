using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.AdminConfig.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.AdminConfig.Exporting
{
    public class BanksExcelExporter : EpPlusExcelExporterBase, IBanksExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BanksExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBankForViewDto> banks)
        {
            return CreateExcelPackage(
                "Banks.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Banks"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("BankName"),
                        L("BankCode"),
                        L("DisplayName"),
                        L("Type"),
                        L("Order"),
                        L("Logo"),
                        L("CardImage"),
                        L("Description")
                        );

                    AddObjects(
                        sheet, 2, banks,
                        _ => _.Bank.BankName,
                        _ => _.Bank.BankCode,
                        _ => _.Bank.DisplayName,
                        _ => _.Bank.Type,
                        _ => _.Bank.Order,
                        _ => _.Bank.Logo,
                        _ => _.Bank.CardImage,
                        _ => _.Bank.Description
                        );

					

                });
        }
    }
}
