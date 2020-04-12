using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Cashout.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Cashout.Exporting
{
    public class CashoutMethodTypesExcelExporter : EpPlusExcelExporterBase, ICashoutMethodTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CashoutMethodTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCashoutMethodTypeForViewDto> cashoutMethodTypes)
        {
            return CreateExcelPackage(
                "CashoutMethodTypes.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("CashoutMethodTypes"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Title"),
                        L("Note")
                        );

                    AddObjects(
                        sheet, 2, cashoutMethodTypes,
                        _ => _.CashoutMethodType.Title,
                        _ => _.CashoutMethodType.Note
                        );

					

                });
        }
    }
}
