using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Products.Exporting
{
    public class TransPortdetailsExcelExporter : EpPlusExcelExporterBase, ITransPortdetailsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TransPortdetailsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTransPortdetailForViewDto> transPortdetails)
        {
            return CreateExcelPackage(
                "TransPortdetails.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("TransPortdetails"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("From"),
                        L("To"),
                        L("TotalSeat"),
                        L("IsTaxi"),
                        (L("Product")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, transPortdetails,
                        _ => _.TransPortdetail.From,
                        _ => _.TransPortdetail.To,
                        _ => _.TransPortdetail.TotalSeat,
                        _ => _.TransPortdetail.IsTaxi,
                        _ => _.ProductName
                        );

					

                });
        }
    }
}
