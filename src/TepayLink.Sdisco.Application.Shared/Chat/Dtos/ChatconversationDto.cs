
using System;
using Abp.Application.Services.Dto;

namespace TepayLink.Sdisco.Chat.Dtos
{
    public class ChatconversationDto : EntityDto<long>
    {
		public long ChatConversationId { get; set; }

		public long UserId { get; set; }

		public string Message { get; set; }

		public int Side { get; set; }

		public int ReadState { get; set; }

		public int ReceiverReadState { get; set; }

		public string SharedMessageId { get; set; }



    }
}