using TepayLink.Sdisco.Chat;
using TepayLink.Sdisco.Chat;
using TepayLink.Sdisco.Chat;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Chat.Dtos
{
    public class CreateOrEditChatMessageV2Dto : EntityDto<long?>
    {

		public long ChatConversationId { get; set; }
		
		
		public long UserId { get; set; }
		
		
		public string Message { get; set; }
		
		
		public DateTime CreationTime { get; set; }
		
		
		public ChatSide Side { get; set; }
		
		
		public ChatMessageReadState ReadState { get; set; }
		
		
		public ChatMessageReadState ReceiverReadState { get; set; }
		
		
		public Guid SharedMessageId { get; set; }
		
		

    }
}