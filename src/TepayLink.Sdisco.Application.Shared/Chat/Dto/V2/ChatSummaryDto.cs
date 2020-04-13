using System;
using System.Collections.Generic;
using System.Text;
using TepayLink.Sdisco.Tour.Dtos;

namespace TepayLink.Sdisco.Chat.Dto.V2
{
    public class ChatSummaryDto
    {
        /// <summary>
        /// Thông tin người chat
        /// </summary>
        public BasicHostUserInfo Friend { get; set; }
        /// <summary>
        /// Id cuộc hội thoại
        /// </summary>
        public string ChatConversationId { get; set; }
        /// <summary>
        /// Tên tour đã book
        /// </summary>
        public string TourTitle { get; set; }
        /// <summary>
        /// Số tin nhắn chưa đọc
        /// </summary>
        public int UnReadCount { get; set; }

        public bool Unread => UnReadCount > 0;

        /// <summary>
        /// Tin nhắn gần nhất
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        ///  ngày gửi gần nhất
        /// </summary>
        public DateTime SentDate { get; set; }
        /// <summary>
        /// Trạng thái booking
        /// </summary>
        public int BookingStatus { get; set; }
        /// <summary>
        /// Id booking
        /// </summary>
        public long BookingId { get; set; }
        /// <summary>
        /// Giá book
        /// </summary>
        public decimal Price { get; set; }

    }
}
