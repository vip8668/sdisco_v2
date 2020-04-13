using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Chat.Dtos;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Chat
{
    public interface IChatMessageV2sAppService : IApplicationService 
    {
        Task<PagedResultDto<GetChatMessageV2ForViewDto>> GetAll(GetAllChatMessageV2sInput input);

        Task<GetChatMessageV2ForViewDto> GetChatMessageV2ForView(long id);

		Task<GetChatMessageV2ForEditOutput> GetChatMessageV2ForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditChatMessageV2Dto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetChatMessageV2sToExcel(GetAllChatMessageV2sForExcelInput input);

		
    }
}