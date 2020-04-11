using System.Collections.Generic;
using TepayLink.Sdisco.Products.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Products.Exporting
{
    public interface IPlacesExcelExporter
    {
        FileDto ExportToFile(List<GetPlaceForViewDto> places);
    }
}