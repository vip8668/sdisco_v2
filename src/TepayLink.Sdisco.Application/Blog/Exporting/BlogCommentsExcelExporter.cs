using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Blog.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Blog.Exporting
{
    public class BlogCommentsExcelExporter : EpPlusExcelExporterBase, IBlogCommentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BlogCommentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBlogCommentForViewDto> blogComments)
        {
            return CreateExcelPackage(
                "BlogComments.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("BlogComments"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Email"),
                        L("FullName"),
                        L("Rating"),
                        L("WebSite"),
                        L("Title"),
                        L("Comment"),
                        (L("BlogPost")) + L("Title")
                        );

                    AddObjects(
                        sheet, 2, blogComments,
                        _ => _.BlogComment.Email,
                        _ => _.BlogComment.FullName,
                        _ => _.BlogComment.Rating,
                        _ => _.BlogComment.WebSite,
                        _ => _.BlogComment.Title,
                        _ => _.BlogComment.Comment,
                        _ => _.BlogPostTitle
                        );

					

                });
        }
    }
}
