using Abp.Application.Services.Dto;
using System;

namespace TepayLink.Sdisco.Chat.Dtos
{
    public class GetAllChatMessageV2sInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public long? MaxChatConversationIdFilter { get; set; }
		public long? MinChatConversationIdFilter { get; set; }

		public long? MaxUserIdFilter { get; set; }
		public long? MinUserIdFilter { get; set; }

		public string MessageFilter { get; set; }

		public DateTime? MaxCreationTimeFilter { get; set; }
		public DateTime? MinCreationTimeFilter { get; set; }

		public int SideFilter { get; set; }

		public int ReadStateFilter { get; set; }

		public int ReceiverReadStateFilter { get; set; }

		public Guid? SharedMessageIdFilter { get; set; }



    }
}