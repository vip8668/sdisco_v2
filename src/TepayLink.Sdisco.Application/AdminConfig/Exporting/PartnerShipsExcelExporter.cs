using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.AdminConfig.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.AdminConfig.Exporting
{
    public class PartnerShipsExcelExporter : EpPlusExcelExporterBase, IPartnerShipsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PartnerShipsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPartnerShipForViewDto> partnerShips)
        {
            return CreateExcelPackage(
                "PartnerShips.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("PartnerShips"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Logo"),
                        L("Title"),
                        L("Link"),
                        L("Order")
                        );

                    AddObjects(
                        sheet, 2, partnerShips,
                        _ => _.PartnerShip.Logo,
                        _ => _.PartnerShip.Title,
                        _ => _.PartnerShip.Link,
                        _ => _.PartnerShip.Order
                        );

					

                });
        }
    }
}
