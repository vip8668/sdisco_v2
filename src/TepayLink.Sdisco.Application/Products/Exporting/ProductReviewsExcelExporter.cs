using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Products.Exporting
{
    public class ProductReviewsExcelExporter : EpPlusExcelExporterBase, IProductReviewsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductReviewsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductReviewForViewDto> productReviews)
        {
            return CreateExcelPackage(
                "ProductReviews.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ProductReviews"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("RatingAvg"),
                        L("ReviewCount"),
                        L("Intineraty"),
                        L("Service"),
                        L("Transport"),
                        L("GuideTour"),
                        L("Food"),
                        (L("Product")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, productReviews,
                        _ => _.ProductReview.RatingAvg,
                        _ => _.ProductReview.ReviewCount,
                        _ => _.ProductReview.Intineraty,
                        _ => _.ProductReview.Service,
                        _ => _.ProductReview.Transport,
                        _ => _.ProductReview.GuideTour,
                        _ => _.ProductReview.Food,
                        _ => _.ProductName
                        );

					

                });
        }
    }
}
