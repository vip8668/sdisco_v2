using System.Collections.Generic;
using TepayLink.Sdisco.Clients.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Clients.Exporting
{
    public interface IClientSettingsExcelExporter
    {
        FileDto ExportToFile(List<GetClientSettingForViewDto> clientSettings);
    }
}