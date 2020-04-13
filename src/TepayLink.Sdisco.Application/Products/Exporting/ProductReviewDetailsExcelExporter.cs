using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Products.Exporting
{
    public class ProductReviewDetailsExcelExporter : EpPlusExcelExporterBase, IProductReviewDetailsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductReviewDetailsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductReviewDetailForViewDto> productReviewDetails)
        {
            return CreateExcelPackage(
                "ProductReviewDetails.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ProductReviewDetails"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("RatingAvg"),
                        L("Intineraty"),
                        L("Service"),
                        L("Transport"),
                        L("GuideTour"),
                        L("Food"),
                        L("Title"),
                        L("Comment"),
                        L("BookingId"),
                        L("Read"),
                        L("ReplyComment"),
                        L("ReplyId"),
                        L("Avatar"),
                        L("Reviewer"),
                        (L("Product")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, productReviewDetails,
                        _ => _.ProductReviewDetail.RatingAvg,
                        _ => _.ProductReviewDetail.Intineraty,
                        _ => _.ProductReviewDetail.Service,
                        _ => _.ProductReviewDetail.Transport,
                        _ => _.ProductReviewDetail.GuideTour,
                        _ => _.ProductReviewDetail.Food,
                        _ => _.ProductReviewDetail.Title,
                        _ => _.ProductReviewDetail.Comment,
                        _ => _.ProductReviewDetail.BookingId,
                        _ => _.ProductReviewDetail.Read,
                        _ => _.ProductReviewDetail.ReplyComment,
                        _ => _.ProductReviewDetail.ReplyId,
                        _ => _.ProductReviewDetail.Avatar,
                        _ => _.ProductReviewDetail.Reviewer,
                        _ => _.ProductName
                        );

					

                });
        }
    }
}
