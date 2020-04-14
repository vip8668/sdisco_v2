using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TepayLink.Sdisco.Chat.Dto.V2;
using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.Chat
{
    public interface IChatV2AppService : IApplicationService
    {
        Task<ChatMessagev2Dto> SendMessage(SendChatMessageV2Dto input);
        Task<int> GetUnreadCount(string ChatConversationId);
        Task<PagedResultDto<ChatSummaryDto>> GetChatSummary(GetChatSummaryDto input);
        Task ArchiveMessage(ArchiveMessageDto input);
        Task ReadMessage(ReadMessageInputDto input);
        Task DeleteMessage(DeleteMessageInputDto input);
        Task<PagedResultDto<ChatMessagev2Dto>> GetChatMessage(GetChatMessageDto input);
        Task<BasicHostUserInfo> GetFriendInfor(long userId);

        Task<List<ChatMessagev2Dto>> SearchChatMessage(SearchMessageDto input);
        Task<bool> CheckUserOnline(long userId);
        Task UpdatUserStaus(bool status);

    }
}
