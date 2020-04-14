using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Reports.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Reports.Exporting
{
    public class RevenueByMonthsExcelExporter : EpPlusExcelExporterBase, IRevenueByMonthsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RevenueByMonthsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRevenueByMonthForViewDto> revenueByMonths)
        {
            return CreateExcelPackage(
                "RevenueByMonths.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("RevenueByMonths"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("UserId"),
                        L("Revenue"),
                        L("Date")
                        );

                    AddObjects(
                        sheet, 2, revenueByMonths,
                        _ => _.RevenueByMonth.UserId,
                        _ => _.RevenueByMonth.Revenue,
                        _ => _timeZoneConverter.Convert(_.RevenueByMonth.Date, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var dateColumn = sheet.Column(3);
                    dateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					dateColumn.AutoFit();
					

                });
        }
    }
}
