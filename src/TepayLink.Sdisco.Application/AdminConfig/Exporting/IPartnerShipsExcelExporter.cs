using System.Collections.Generic;
using TepayLink.Sdisco.AdminConfig.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.AdminConfig.Exporting
{
    public interface IPartnerShipsExcelExporter
    {
        FileDto ExportToFile(List<GetPartnerShipForViewDto> partnerShips);
    }
}