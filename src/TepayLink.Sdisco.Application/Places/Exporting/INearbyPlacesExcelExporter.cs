using System.Collections.Generic;
using TepayLink.Sdisco.Places.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Places.Exporting
{
    public interface INearbyPlacesExcelExporter
    {
        FileDto ExportToFile(List<GetNearbyPlaceForViewDto> nearbyPlaces);
    }
}