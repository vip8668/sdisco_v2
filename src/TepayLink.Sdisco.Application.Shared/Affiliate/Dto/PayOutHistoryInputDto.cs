
using System;
using System.Collections.Generic;
using System.Text;
using TepayLink.Sdisco.Dto;

namespace SDisco.Affiliate.Dto
{
    public class PayOutInputDto: PagedInputDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
