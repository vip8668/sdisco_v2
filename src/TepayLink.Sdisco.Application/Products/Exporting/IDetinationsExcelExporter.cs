using System.Collections.Generic;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Products.Exporting
{
    public interface IDetinationsExcelExporter
    {
        FileDto ExportToFile(List<GetDetinationForViewDto> detinations);
    }
}