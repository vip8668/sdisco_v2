
using TepayLink.Sdisco.Chat;

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
			var sideFilter = (ChatSide) input.SideFilter;
			
			var filteredChatconversations = _chatconversationRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.ShardChatConversationId.Contains(input.Filter) || e.LastMessage.Contains(input.Filter))
						.WhereIf(input.MinUserIdFilter != null, e => e.UserId >= input.MinUserIdFilter)
						.WhereIf(input.MaxUserIdFilter != null, e => e.UserId <= input.MaxUserIdFilter)
						.WhereIf(input.MinFriendUserIdFilter != null, e => e.FriendUserId >= input.MinFriendUserIdFilter)
						.WhereIf(input.MaxFriendUserIdFilter != null, e => e.FriendUserId <= input.MaxFriendUserIdFilter)
						.WhereIf(input.MinUnreadCountFilter != null, e => e.UnreadCount >= input.MinUnreadCountFilter)
						.WhereIf(input.MaxUnreadCountFilter != null, e => e.UnreadCount <= input.MaxUnreadCountFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ShardChatConversationIdFilter),  e => e.ShardChatConversationId == input.ShardChatConversationIdFilter)
						.WhereIf(input.MinBookingIdFilter != null, e => e.BookingId >= input.MinBookingIdFilter)
						.WhereIf(input.MaxBookingIdFilter != null, e => e.BookingId <= input.MaxBookingIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LastMessageFilter),  e => e.LastMessage == input.LastMessageFilter)
						.WhereIf(input.SideFilter > -1, e => e.Side == sideFilter)
						.WhereIf(input.MinStatusFilter != null, e => e.Status >= input.MinStatusFilter)
						.WhereIf(input.MaxStatusFilter != null, e => e.Status <= input.MaxStatusFilter);

			var pagedAndFilteredChatconversations = filteredChatconversations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var chatconversations = from o in pagedAndFilteredChatconversations
                         select new GetChatconversationForViewDto() {
							Chatconversation = new ChatconversationDto
							{
                                UserId = o.UserId,
                                FriendUserId = o.FriendUserId,
                                UnreadCount = o.UnreadCount,
                                ShardChatConversationId = o.ShardChatConversationId,
                                BookingId = o.BookingId,
                                LastMessage = o.LastMessage,
                                Side = o.Side,
                                Status = o.Status,
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