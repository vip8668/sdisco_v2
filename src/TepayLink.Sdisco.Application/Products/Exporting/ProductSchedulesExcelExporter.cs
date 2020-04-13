using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Products.Exporting
{
    public class ProductSchedulesExcelExporter : EpPlusExcelExporterBase, IProductSchedulesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductSchedulesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductScheduleForViewDto> productSchedules)
        {
            return CreateExcelPackage(
                "ProductSchedules.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ProductSchedules"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("TotalSlot"),
                        L("TotalBook"),
                        L("LockedSlot"),
                        L("TripLength"),
                        L("Note"),
                        L("Price"),
                        L("TicketPrice"),
                        L("CostPrice"),
                        L("HotelPrice"),
                        L("StartDate"),
                        L("EndDate"),
                        L("DepartureTime"),
                        L("Revenue"),
                        L("AllowBook"),
                        (L("Product")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, productSchedules,
                        _ => _.ProductSchedule.TotalSlot,
                        _ => _.ProductSchedule.TotalBook,
                        _ => _.ProductSchedule.LockedSlot,
                        _ => _.ProductSchedule.TripLength,
                        _ => _.ProductSchedule.Note,
                        _ => _.ProductSchedule.Price,
                        _ => _.ProductSchedule.TicketPrice,
                        _ => _.ProductSchedule.CostPrice,
                        _ => _.ProductSchedule.HotelPrice,
                        _ => _timeZoneConverter.Convert(_.ProductSchedule.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.ProductSchedule.EndDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ProductSchedule.DepartureTime,
                        _ => _.ProductSchedule.Revenue,
                        _ => _.ProductSchedule.AllowBook,
                        _ => _.ProductName
                        );

					var startDateColumn = sheet.Column(10);
                    startDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					startDateColumn.AutoFit();
					var endDateColumn = sheet.Column(11);
                    endDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					endDateColumn.AutoFit();
					

                });
        }
    }
}
