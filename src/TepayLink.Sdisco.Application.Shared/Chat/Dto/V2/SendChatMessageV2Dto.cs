using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Chat.Dto.V2
{
    public class SendChatMessageV2Dto
    {
        /// <summary>
        /// Id cuôc hội thoại. nếu bắt đầu 1 cuộc hộ thoại mới để trống
        /// </summary>
        public string ChatConversationId { get; set; }
        /// <summary>
        /// ID người chát cùng
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// Tin nhắn
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Booking Id, Truyền khi bắt đầu cuộc hộ thoại mới, khi có thông tin booking đi kèm
        /// </summary>
        public long? BookingId { get; set; }
    }
}
