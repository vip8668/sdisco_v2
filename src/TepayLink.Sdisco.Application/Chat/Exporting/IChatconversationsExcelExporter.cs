using System.Collections.Generic;
using TepayLink.Sdisco.Chat.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Chat.Exporting
{
    public interface IChatconversationsExcelExporter
    {
        FileDto ExportToFile(List<GetChatconversationForViewDto> chatconversations);
    }
}