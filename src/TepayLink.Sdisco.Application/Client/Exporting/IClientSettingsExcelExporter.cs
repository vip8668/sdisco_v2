using System.Collections.Generic;
using TepayLink.Sdisco.Client.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Client.Exporting
{
    public interface IClientSettingsExcelExporter
    {
        FileDto ExportToFile(List<GetClientSettingForViewDto> clientSettings);
    }
}