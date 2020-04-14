using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Bookings.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Bookings.Exporting
{
    public class RefundReasonsExcelExporter : EpPlusExcelExporterBase, IRefundReasonsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RefundReasonsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRefundReasonForViewDto> refundReasons)
        {
            return CreateExcelPackage(
                "RefundReasons.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("RefundReasons"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("ReasonText")
                        );

                    AddObjects(
                        sheet, 2, refundReasons,
                        _ => _.RefundReason.ReasonText
                        );

					

                });
        }
    }
}
