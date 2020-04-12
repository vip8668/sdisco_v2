using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Account.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Account.Exporting
{
    public class UserReviewsExcelExporter : EpPlusExcelExporterBase, IUserReviewsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public UserReviewsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetUserReviewForViewDto> userReviews)
        {
            return CreateExcelPackage(
                "UserReviews.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("UserReviews"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("UserId"),
                        L("ReviewCount"),
                        L("Itineraty"),
                        L("Service"),
                        L("Transport"),
                        L("GuideTour"),
                        L("Food"),
                        L("Rating")
                        );

                    AddObjects(
                        sheet, 2, userReviews,
                        _ => _.UserReview.UserId,
                        _ => _.UserReview.ReviewCount,
                        _ => _.UserReview.Itineraty,
                        _ => _.UserReview.Service,
                        _ => _.UserReview.Transport,
                        _ => _.UserReview.GuideTour,
                        _ => _.UserReview.Food,
                        _ => _.UserReview.Rating
                        );

					

                });
        }
    }
}
