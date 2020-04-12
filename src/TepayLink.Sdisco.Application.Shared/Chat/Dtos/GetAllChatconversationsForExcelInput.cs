using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Chat.Dtos
{
    public class GetAllChatconversationsForExcelInput
    {
		public string Filter { get; set; }

		public long? MaxFriendUserIdFilter { get; set; }
		public long? MinFriendUserIdFilter { get; set; }

		public int? MaxUnreadCountFilter { get; set; }
		public int? MinUnreadCountFilter { get; set; }

		public string ShardChatConversationIdFilter { get; set; }

		public long? MaxBookingIdFilter { get; set; }
		public long? MinBookingIdFilter { get; set; }

		public string LastMessageFilter { get; set; }

		public int? MaxSideFilter { get; set; }
		public int? MinSideFilter { get; set; }

		public int? MaxStatusFilter { get; set; }
		public int? MinStatusFilter { get; set; }



    }
}