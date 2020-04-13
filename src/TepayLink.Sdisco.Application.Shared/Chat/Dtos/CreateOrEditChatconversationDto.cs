using TepayLink.Sdisco.Chat;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Chat.Dtos
{
    public class CreateOrEditChatconversationDto : EntityDto<long?>
    {

		public long UserId { get; set; }
		
		
		public long FriendUserId { get; set; }
		
		
		public int UnreadCount { get; set; }
		
		
		public string ShardChatConversationId { get; set; }
		
		
		public long? BookingId { get; set; }
		
		
		public string LastMessage { get; set; }
		
		
		public ChatSide Side { get; set; }
		
		
		public int Status { get; set; }
		
		

    }
}