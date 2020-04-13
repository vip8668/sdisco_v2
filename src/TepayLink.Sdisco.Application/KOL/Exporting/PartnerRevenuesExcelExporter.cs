using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.KOL.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.KOL.Exporting
{
    public class PartnerRevenuesExcelExporter : EpPlusExcelExporterBase, IPartnerRevenuesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PartnerRevenuesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPartnerRevenueForViewDto> partnerRevenues)
        {
            return CreateExcelPackage(
                "PartnerRevenues.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("PartnerRevenues"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Userid"),
                        L("RevenueType"),
                        L("ProductId"),
                        L("Point"),
                        L("Money"),
                        L("Status")
                        );

                    AddObjects(
                        sheet, 2, partnerRevenues,
                        _ => _.PartnerRevenue.Userid,
                        _ => _.PartnerRevenue.RevenueType,
                        _ => _.PartnerRevenue.ProductId,
                        _ => _.PartnerRevenue.Point,
                        _ => _.PartnerRevenue.Money,
                        _ => _.PartnerRevenue.Status
                        );

					

                });
        }
    }
}
