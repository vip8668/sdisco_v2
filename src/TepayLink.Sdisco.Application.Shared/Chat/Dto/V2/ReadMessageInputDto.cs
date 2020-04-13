using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Chat.Dto.V2
{
    public class ReadMessageInputDto
    {
        /// <summary>
        /// Id cuộc hộ thoại
        /// </summary>
        public string ChatConversationId { get; set; }
    }
    public class DeleteMessageInputDto
    {
        /// <summary>
        /// Id cuộc hộ thoại
        /// </summary>
        public string ChatConversationId { get; set; }
    }
}
