using System;
using System.Collections.Generic;
using System.Text;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Chat.Dto.V2
{
    public class GetChatSummaryDto : PagedInputDto
    {
        /// <summary>
        /// 0: tất cả, 1 Đã đọc, 2 chưa đọc
        /// </summary>
        public int Status { get; set; }
    }
}
