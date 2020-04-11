using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TepayLink.Sdisco.DataExporting.Excel.EpPlus;
using TepayLink.Sdisco.Blog.Dtos;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Storage;

namespace TepayLink.Sdisco.Blog.Exporting
{
    public class BlogProductRelatedsExcelExporter : EpPlusExcelExporterBase, IBlogProductRelatedsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BlogProductRelatedsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBlogProductRelatedForViewDto> blogProductRelateds)
        {
            return CreateExcelPackage(
                "BlogProductRelateds.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("BlogProductRelateds"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        (L("BlogPost")) + L("Title"),
                        (L("Product")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, blogProductRelateds,
                        _ => _.BlogPostTitle,
                        _ => _.ProductName
                        );

					

                });
        }
    }
}
