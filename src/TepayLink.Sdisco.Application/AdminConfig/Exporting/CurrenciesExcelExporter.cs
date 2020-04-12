using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.AdminConfig.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.AdminConfig.Exporting
{
    public class CurrenciesExcelExporter : EpPlusExcelExporterBase, ICurrenciesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CurrenciesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCurrencyForViewDto> currencies)
        {
            return CreateExcelPackage(
                "Currencies.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Currencies"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("DisplayName"),
                        L("Icon"),
                        L("CurrencySign"),
                        L("IsDisabled")
                        );

                    AddObjects(
                        sheet, 2, currencies,
                        _ => _.Currency.Name,
                        _ => _.Currency.DisplayName,
                        _ => _.Currency.Icon,
                        _ => _.Currency.CurrencySign,
                        _ => _.Currency.IsDisabled
                        );

					

                });
        }
    }
}
