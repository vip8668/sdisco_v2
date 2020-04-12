using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Chat.Dtos
{
    public class GetAllChatconversationsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public long? MaxChatConversationIdFilter { get; set; }
		public long? MinChatConversationIdFilter { get; set; }

		public long? MaxUserIdFilter { get; set; }
		public long? MinUserIdFilter { get; set; }

		public string MessageFilter { get; set; }

		public int? MaxSideFilter { get; set; }
		public int? MinSideFilter { get; set; }

		public int? MaxReadStateFilter { get; set; }
		public int? MinReadStateFilter { get; set; }

		public int? MaxReceiverReadStateFilter { get; set; }
		public int? MinReceiverReadStateFilter { get; set; }

		public string SharedMessageIdFilter { get; set; }



    }
}