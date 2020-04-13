
using TepayLink.Sdisco.Chat;
using TepayLink.Sdisco.Chat;
using TepayLink.Sdisco.Chat;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TepayLink.Sdisco.Chat.Exporting;
using TepayLink.Sdisco.Chat.Dtos;
using TepayLink.Sdisco.Dto;
using Abp.Application.Services.Dto;
using TepayLink.Sdisco.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TepayLink.Sdisco.Chat
{
	[AbpAuthorize(AppPermissions.Pages_ChatMessageV2s)]
    public class ChatMessageV2sAppService : SdiscoAppServiceBase, IChatMessageV2sAppService
    {
		 private readonly IRepository<ChatMessageV2, long> _chatMessageV2Repository;
		 private readonly IChatMessageV2sExcelExporter _chatMessageV2sExcelExporter;
		 

		  public ChatMessageV2sAppService(IRepository<ChatMessageV2, long> chatMessageV2Repository, IChatMessageV2sExcelExporter chatMessageV2sExcelExporter ) 
		  {
			_chatMessageV2Repository = chatMessageV2Repository;
			_chatMessageV2sExcelExporter = chatMessageV2sExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetChatMessageV2ForViewDto>> GetAll(GetAllChatMessageV2sInput input)
         {
			var sideFilter = (ChatSide) input.SideFilter;
			var readStateFilter = (ChatMessageReadState) input.ReadStateFilter;
			var receiverReadStateFilter = (ChatMessageReadState) input.ReceiverReadStateFilter;
			
			var filteredChatMessageV2s = _chatMessageV2Repository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Message.Contains(input.Filter))
						.WhereIf(input.MinChatConversationIdFilter != null, e => e.ChatConversationId >= input.MinChatConversationIdFilter)
						.WhereIf(input.MaxChatConversationIdFilter != null, e => e.ChatConversationId <= input.MaxChatConversationIdFilter)
						.WhereIf(input.MinUserIdFilter != null, e => e.UserId >= input.MinUserIdFilter)
						.WhereIf(input.MaxUserIdFilter != null, e => e.UserId <= input.MaxUserIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MessageFilter),  e => e.Message == input.MessageFilter)
						.WhereIf(input.MinCreationTimeFilter != null, e => e.CreationTime >= input.MinCreationTimeFilter)
						.WhereIf(input.MaxCreationTimeFilter != null, e => e.CreationTime <= input.MaxCreationTimeFilter)
						.WhereIf(input.SideFilter > -1, e => e.Side == sideFilter)
						.WhereIf(input.ReadStateFilter > -1, e => e.ReadState == readStateFilter)
						.WhereIf(input.ReceiverReadStateFilter > -1, e => e.ReceiverReadState == receiverReadStateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SharedMessageIdFilter.ToString()),  e => e.SharedMessageId.ToString() == input.SharedMessageIdFilter.ToString());

			var pagedAndFilteredChatMessageV2s = filteredChatMessageV2s
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var chatMessageV2s = from o in pagedAndFilteredChatMessageV2s
                         select new GetChatMessageV2ForViewDto() {
							ChatMessageV2 = new ChatMessageV2Dto
							{
                                ChatConversationId = o.ChatConversationId,
                                UserId = o.UserId,
                                Message = o.Message,
                                CreationTime = o.CreationTime,
                                Side = o.Side,
                                ReadState = o.ReadState,
                                ReceiverReadState = o.ReceiverReadState,
                                SharedMessageId = o.SharedMessageId,
                                Id = o.Id
							}
						};

            var totalCount = await filteredChatMessageV2s.CountAsync();

            return new PagedResultDto<GetChatMessageV2ForViewDto>(
                totalCount,
                await chatMessageV2s.ToListAsync()
            );
         }
		 
		 public async Task<GetChatMessageV2ForViewDto> GetChatMessageV2ForView(long id)
         {
            var chatMessageV2 = await _chatMessageV2Repository.GetAsync(id);

            var output = new GetChatMessageV2ForViewDto { ChatMessageV2 = ObjectMapper.Map<ChatMessageV2Dto>(chatMessageV2) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ChatMessageV2s_Edit)]
		 public async Task<GetChatMessageV2ForEditOutput> GetChatMessageV2ForEdit(EntityDto<long> input)
         {
            var chatMessageV2 = await _chatMessageV2Repository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetChatMessageV2ForEditOutput {ChatMessageV2 = ObjectMapper.Map<CreateOrEditChatMessageV2Dto>(chatMessageV2)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditChatMessageV2Dto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ChatMessageV2s_Create)]
		 protected virtual async Task Create(CreateOrEditChatMessageV2Dto input)
         {
            var chatMessageV2 = ObjectMapper.Map<ChatMessageV2>(input);

			
			if (AbpSession.TenantId != null)
			{
				chatMessageV2.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _chatMessageV2Repository.InsertAsync(chatMessageV2);
         }

		 [AbpAuthorize(AppPermissions.Pages_ChatMessageV2s_Edit)]
		 protected virtual async Task Update(CreateOrEditChatMessageV2Dto input)
         {
            var chatMessageV2 = await _chatMessageV2Repository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, chatMessageV2);
         }

		 [AbpAuthorize(AppPermissions.Pages_ChatMessageV2s_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _chatMessageV2Repository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetChatMessageV2sToExcel(GetAllChatMessageV2sForExcelInput input)
         {
			var sideFilter = (ChatSide) input.SideFilter;
			var readStateFilter = (ChatMessageReadState) input.ReadStateFilter;
			var receiverReadStateFilter = (ChatMessageReadState) input.ReceiverReadStateFilter;
			
			var filteredChatMessageV2s = _chatMessageV2Repository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Message.Contains(input.Filter))
						.WhereIf(input.MinChatConversationIdFilter != null, e => e.ChatConversationId >= input.MinChatConversationIdFilter)
						.WhereIf(input.MaxChatConversationIdFilter != null, e => e.ChatConversationId <= input.MaxChatConversationIdFilter)
						.WhereIf(input.MinUserIdFilter != null, e => e.UserId >= input.MinUserIdFilter)
						.WhereIf(input.MaxUserIdFilter != null, e => e.UserId <= input.MaxUserIdFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MessageFilter),  e => e.Message == input.MessageFilter)
						.WhereIf(input.MinCreationTimeFilter != null, e => e.CreationTime >= input.MinCreationTimeFilter)
						.WhereIf(input.MaxCreationTimeFilter != null, e => e.CreationTime <= input.MaxCreationTimeFilter)
						.WhereIf(input.SideFilter > -1, e => e.Side == sideFilter)
						.WhereIf(input.ReadStateFilter > -1, e => e.ReadState == readStateFilter)
						.WhereIf(input.ReceiverReadStateFilter > -1, e => e.ReceiverReadState == receiverReadStateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SharedMessageIdFilter.ToString()),  e => e.SharedMessageId.ToString() == input.SharedMessageIdFilter.ToString());

			var query = (from o in filteredChatMessageV2s
                         select new GetChatMessageV2ForViewDto() { 
							ChatMessageV2 = new ChatMessageV2Dto
							{
                                ChatConversationId = o.ChatConversationId,
                                UserId = o.UserId,
                                Message = o.Message,
                                CreationTime = o.CreationTime,
                                Side = o.Side,
                                ReadState = o.ReadState,
                                ReceiverReadState = o.ReceiverReadState,
                                SharedMessageId = o.SharedMessageId,
                                Id = o.Id
							}
						 });


            var chatMessageV2ListDtos = await query.ToListAsync();

            return _chatMessageV2sExcelExporter.ExportToFile(chatMessageV2ListDtos);
         }


    }
}