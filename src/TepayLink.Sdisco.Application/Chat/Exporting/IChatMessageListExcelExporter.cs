using System.Collections.Generic;
using Abp;
using TepayLink.Sdisco.Chat.Dto;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
