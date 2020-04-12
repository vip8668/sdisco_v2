using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Account.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Account.Exporting
{
    public class UserReviewDetailsExcelExporter : EpPlusExcelExporterBase, IUserReviewDetailsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public UserReviewDetailsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetUserReviewDetailForViewDto> userReviewDetails)
        {
            return CreateExcelPackage(
                "UserReviewDetails.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("UserReviewDetails"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("UserId"),
                        L("Itineraty"),
                        L("Service"),
                        L("Transport"),
                        L("GuideTour"),
                        L("Food"),
                        L("Rating"),
                        L("Title"),
                        L("Comment")
                        );

                    AddObjects(
                        sheet, 2, userReviewDetails,
                        _ => _.UserReviewDetail.UserId,
                        _ => _.UserReviewDetail.Itineraty,
                        _ => _.UserReviewDetail.Service,
                        _ => _.UserReviewDetail.Transport,
                        _ => _.UserReviewDetail.GuideTour,
                        _ => _.UserReviewDetail.Food,
                        _ => _.UserReviewDetail.Rating,
                        _ => _.UserReviewDetail.Title,
                        _ => _.UserReviewDetail.Comment
                        );

					

                });
        }
    }
}
