using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Chat.Dtos
{
    public class GetAllChatconversationsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public long? MaxUserIdFilter { get; set; }
		public long? MinUserIdFilter { get; set; }

		public long? MaxFriendUserIdFilter { get; set; }
		public long? MinFriendUserIdFilter { get; set; }

		public int? MaxUnreadCountFilter { get; set; }
		public int? MinUnreadCountFilter { get; set; }

		public string ShardChatConversationIdFilter { get; set; }

		public long? MaxBookingIdFilter { get; set; }
		public long? MinBookingIdFilter { get; set; }

		public string LastMessageFilter { get; set; }

		public int SideFilter { get; set; }

		public int? MaxStatusFilter { get; set; }
		public int? MinStatusFilter { get; set; }



    }
}