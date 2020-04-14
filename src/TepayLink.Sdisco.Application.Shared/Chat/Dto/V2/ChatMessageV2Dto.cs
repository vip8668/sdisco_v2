using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Chat.Dto.V2
{
    public class ChatMessagev2Dto
    {
        public long Id { get; set; }

        /// <summary>
        /// side : 1 gửi, 2 nhận
        /// </summary>
        public ChatSide Side { get; set; }
        /// <summary>
        /// Tin nhắn
        /// </summary>

        public string Message { get; set; }
        /// <summary>
        /// Thời gian nhận
        /// </summary>

        public DateTime CreationTime { get; set; }

        public Guid? SharedMessageId { get; set; }
        public string ChatConversationId { get; set; }
    }
}
