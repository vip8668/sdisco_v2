using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Blog.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Blog.Exporting
{
    public class BlogPostsExcelExporter : EpPlusExcelExporterBase, IBlogPostsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BlogPostsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBlogPostForViewDto> blogPosts)
        {
            return CreateExcelPackage(
                "BlogPosts.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("BlogPosts"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Title"),
                        L("ShortDescription"),
                        L("Content"),
                        L("PublishDate"),
                        L("ThumbImage"),
                        L("Status")
                        );

                    AddObjects(
                        sheet, 2, blogPosts,
                        _ => _.BlogPost.Title,
                        _ => _.BlogPost.ShortDescription,
                        _ => _.BlogPost.Content,
                        _ => _timeZoneConverter.Convert(_.BlogPost.PublishDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.BlogPost.ThumbImage,
                        _ => _.BlogPost.Status
                        );

					var publishDateColumn = sheet.Column(4);
                    publishDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					publishDateColumn.AutoFit();
					

                });
        }
    }
}
