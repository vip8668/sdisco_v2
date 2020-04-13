using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Affiliate.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Affiliate.Exporting
{
    public class ShortLinksExcelExporter : EpPlusExcelExporterBase, IShortLinksExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ShortLinksExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetShortLinkForViewDto> shortLinks)
        {
            return CreateExcelPackage(
                "ShortLinks.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ShortLinks"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("UserId"),
                        L("FullLink"),
                        L("ShortCode")
                        );

                    AddObjects(
                        sheet, 2, shortLinks,
                        _ => _.ShortLink.UserId,
                        _ => _.ShortLink.FullLink,
                        _ => _.ShortLink.ShortCode
                        );

					

                });
        }
    }
}
