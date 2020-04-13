using System.Collections.Generic;
using TepayLink.Sdisco.Chat.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Chat.Exporting
{
    public interface IChatMessageV2sExcelExporter
    {
        FileDto ExportToFile(List<GetChatMessageV2ForViewDto> chatMessageV2s);
    }
}