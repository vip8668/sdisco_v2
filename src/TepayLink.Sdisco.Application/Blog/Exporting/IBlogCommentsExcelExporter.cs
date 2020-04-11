using System.Collections.Generic;
using TepayLink.Sdisco.Blog.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Blog.Exporting
{
    public interface IBlogCommentsExcelExporter
    {
        FileDto ExportToFile(List<GetBlogCommentForViewDto> blogComments);
    }
}