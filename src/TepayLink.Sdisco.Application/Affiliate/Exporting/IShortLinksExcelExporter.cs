using System.Collections.Generic;
using TepayLink.Sdisco.Affiliate.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Affiliate.Exporting
{
    public interface IShortLinksExcelExporter
    {
        FileDto ExportToFile(List<GetShortLinkForViewDto> shortLinks);
    }
}