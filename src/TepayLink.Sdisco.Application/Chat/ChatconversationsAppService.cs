

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TepayLink.Sdisco.Chat.Dtos;
using TepayLink.Sdisco.Dto;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TepayLink.Sdisco.Chat
{
	[AbpAuthorize(AppPermissions.Pages_Chatconversations)]
    public class ChatconversationsAppService : SdiscoAppServiceBase, IChatconversationsAppService
    {
		 private readonly IRepository<Chatconversation, long> _chatconversationRepository;
		 

		  public ChatconversationsAppService(IRepository<Chatconversation, long> chatconversationRepository ) 
		  {
			_chatconversationRepository = chatconversationRepository;
			
		  }

		 public async Task<PagedResultDto<GetChatconversationForViewDto>> GetAll(GetAllChatconversationsInput input)
         {
			
			var filteredChatconversations = _chatconversationRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Message.Contains(input.Filter) || e.SharedMessageId.Contains(input.Filter))
						.WhereIf(input.MinChatConversationIdFilter != null, e => e.ChatConversationId >= input.MinChatConversationIdFilter)
						.WhereIf(input.MaxChatConversationIdFilter != null, e => e.ChatConversationId <= input.MaxChatConversationIdFilter)
						.WhereIf(input.MinUserIdFilter != null, e => e.UserId >= input.MinUserIdFilter)
						.WhereIf(input.MaxUserIdFilter != null, e => e.UserId <= input.MaxUserIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MessageFilter),  e => e.Message == input.MessageFilter)
						.WhereIf(input.MinSideFilter != null, e => e.Side >= input.MinSideFilter)
						.WhereIf(input.MaxSideFilter != null, e => e.Side <= input.MaxSideFilter)
						.WhereIf(input.MinReadStateFilter != null, e => e.ReadState >= input.MinReadStateFilter)
						.WhereIf(input.MaxReadStateFilter != null, e => e.ReadState <= input.MaxReadStateFilter)
						.WhereIf(input.MinReceiverReadStateFilter != null, e => e.ReceiverReadState >= input.MinReceiverReadStateFilter)
						.WhereIf(input.MaxReceiverReadStateFilter != null, e => e.ReceiverReadState <= input.MaxReceiverReadStateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SharedMessageIdFilter),  e => e.SharedMessageId == input.SharedMessageIdFilter);

			var pagedAndFilteredChatconversations = filteredChatconversations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var chatconversations = from o in pagedAndFilteredChatconversations
                         select new GetChatconversationForViewDto() {
							Chatconversation = new ChatconversationDto
							{
                                ChatConversationId = o.ChatConversationId,
                                UserId = o.UserId,
                                Message = o.Message,
                                Side = o.Side,
                                ReadState = o.ReadState,
                                ReceiverReadState = o.ReceiverReadState,
                                SharedMessageId = o.SharedMessageId,
                                Id = o.Id
							}
						};

            var totalCount = await filteredChatconversations.CountAsync();

            return new PagedResultDto<GetChatconversationForViewDto>(
                totalCount,
                await chatconversations.ToListAsync()
            );
         }
		 
		 public async Task<GetChatconversationForViewDto> GetChatconversationForView(long id)
         {
            var chatconversation = await _chatconversationRepository.GetAsync(id);

            var output = new GetChatconversationForViewDto { Chatconversation = ObjectMapper.Map<ChatconversationDto>(chatconversation) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Chatconversations_Edit)]
		 public async Task<GetChatconversationForEditOutput> GetChatconversationForEdit(EntityDto<long> input)
         {
            var chatconversation = await _chatconversationRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetChatconversationForEditOutput {Chatconversation = ObjectMapper.Map<CreateOrEditChatconversationDto>(chatconversation)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditChatconversationDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Chatconversations_Create)]
		 protected virtual async Task Create(CreateOrEditChatconversationDto input)
         {
            var chatconversation = ObjectMapper.Map<Chatconversation>(input);

			
			if (AbpSession.TenantId != null)
			{
				chatconversation.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _chatconversationRepository.InsertAsync(chatconversation);
         }

		 [AbpAuthorize(AppPermissions.Pages_Chatconversations_Edit)]
		 protected virtual async Task Update(CreateOrEditChatconversationDto input)
         {
            var chatconversation = await _chatconversationRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, chatconversation);
         }

		 [AbpAuthorize(AppPermissions.Pages_Chatconversations_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _chatconversationRepository.DeleteAsync(input.Id);
         } 
    }
}