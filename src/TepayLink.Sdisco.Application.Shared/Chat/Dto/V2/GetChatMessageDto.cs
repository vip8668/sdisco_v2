using System;
using System.Collections.Generic;
using System.Text;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Chat.Dto.V2
{
    public class GetChatMessageDto : PagedInputDto
    {

        public string ChatConversationId { get; set; }

        /// <summary>
        /// Id messgae load gần nhấn, nếu chưa có để 0
        /// </summary>
        public long LastId { get; set; }
    }
    public class SearchMessageDto
    {
        public string ChatConversationId { get; set; }
        /// <summary>
        /// Id messgae load gần nhấn, nếu chưa có để 0
        /// </summary>
        public string KeyWord { get; set; }
    }
}
