using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Chat.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Chat
{
    public interface IChatconversationsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetChatconversationForViewDto>> GetAll(GetAllChatconversationsInput input);

        Task<GetChatconversationForViewDto> GetChatconversationForView(long id);

		Task<GetChatconversationForEditOutput> GetChatconversationForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditChatconversationDto input);

		Task Delete(EntityDto<long> input);

		
    }
}