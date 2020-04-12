using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Cashout.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Cashout.Exporting
{
    public class UserDefaultCashoutMethodTypesExcelExporter : EpPlusExcelExporterBase, IUserDefaultCashoutMethodTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public UserDefaultCashoutMethodTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetUserDefaultCashoutMethodTypeForViewDto> userDefaultCashoutMethodTypes)
        {
            return CreateExcelPackage(
                "UserDefaultCashoutMethodTypes.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("UserDefaultCashoutMethodTypes"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        (L("CashoutMethodType")) + L("Title"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, userDefaultCashoutMethodTypes,
                        _ => _.CashoutMethodTypeTitle,
                        _ => _.UserName
                        );

					

                });
        }
    }
}
